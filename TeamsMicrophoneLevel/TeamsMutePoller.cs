using BaristaLabs.ChromeDevTools.Runtime.DOM;
using Newtonsoft.Json;

namespace TeamsMicrophoneLevel
{
    internal class TeamsMutePoller : IDisposable
    {
        private readonly int _debugPort;
        private bool _isConnected = false;
        private bool _isSessionActive = false;
        private bool _isMicrophoneOn = false;
        private readonly object _stateLock = new();
        private DateTime _lastDevicePoll = DateTime.MinValue;
        private readonly TimeSpan _connectedPollInterval = TimeSpan.FromSeconds(5);
        private readonly Dictionary<string, ChromeSessionState?> _sessions = new();
        private static readonly TimeSpan _timeout = TimeSpan.FromMilliseconds(3000);


        /// <summary>
        /// Construct with the port of the debug server.
        /// </summary>
        /// <remarks>
        /// Run teams with "--remote-debugging-port=<port>"
        /// </remarks>
        public TeamsMutePoller(int port)
        {
            _debugPort = port;
        }

        public void Dispose()
        {
            ClearSessions();
        }

        private void ClearSessions()
        {
            var ids = _sessions.Keys.ToList();
            foreach (var id in ids)
            {
                var session = _sessions[id];
                if (session != null)
                {
                    session.Dispose();
                    _sessions.Remove(id);
                }
            }
            _sessions.Clear();
        }

        /// <summary>
        /// Poll the teams electron process (running in debug mode) to see if the mute
        /// button is clicked.
        /// </summary>
        public async Task Poll(CancellationToken token)
        {
            await ConnectSessions(token);
            await CheckSessionsMicrophoneOn(token);
        }

        /// <summary>
        /// Accessor for the configured debug port to poll.
        /// </summary>
        public int DebugPort => _debugPort;

        /// <summary>
        /// Check to see if we are connected to the teams debug port.
        /// </summary>
        public bool IsStatusConnected
        {
            get
            {
                lock (_stateLock)
                {
                    return _isConnected;
                }
            }
        }

        /// <summary>
        /// Count of debug sessions (tabs) we can see in teams.
        /// </summary>
        public int Sessions => _sessions.Count;

        /// <summary>
        /// Check to see if a call is running.
        /// </summary>
        /// <remarks>
        /// ie we could connect to the debug server
        /// and we found an active call tab
        /// </remarks>
        public bool IsCallActive
        {
            get
            {
                lock (_stateLock)
                {
                    return _isConnected && _isSessionActive;
                }
            }
        }

        /// <summary>
        /// Check to see if the microphone is on in a call.
        /// </summary>
        public bool IsMicrophoneOn
        {
            get
            {
                lock (_stateLock)
                {
                    return _isMicrophoneOn;
                }
            }
        }


        /// <summary>
        /// Connect to teams dev tools and connect debugger sessions.
        /// </summary>
        private async Task ConnectSessions(CancellationToken token)
        {
            // exit early if we have recently checked successfully
            if (_isConnected && DateTime.UtcNow.Subtract(_lastDevicePoll) < _connectedPollInterval)
            {
                return;
            }
            _lastDevicePoll = DateTime.UtcNow;

            try
            {
                using (var webClient = new HttpClient())
                {
                    // connect to the debug server
                    webClient.BaseAddress = new Uri($"http://localhost:{_debugPort}");
                    var remoteSessions = await webClient.GetStringAsync("/json");
                    _isConnected = true;

                    // parse response
                    var sessionsInfo = JsonConvert.DeserializeObject<ICollection<ChromeSessionInfo>>(remoteSessions);
                    if (sessionsInfo == null)
                    {
                        return;
                    }

                    // iterate sessions (tabs)
                    foreach (var sessionInfo in sessionsInfo.Where(x => x.Type == "page"))
                    {
                        await CheckAndAddSession(sessionInfo.Id, sessionInfo.WebSocketDebuggerUrl, token);
                    }
                }
            }
            catch(Exception)
            {
                ClearSessions();
                _isConnected = false;
                _isSessionActive = false;
                _isMicrophoneOn = false;
            }
        }

        /// <summary>
        /// Add a session if it contains the mute button we want to inspect.
        /// </summary>
        private async Task CheckAndAddSession(string? id, string? debuggerUrl, CancellationToken token)
        {
            // skip invalid sessions
            if (id == null || debuggerUrl == null)
            {
                return;
            }

            // skip if we already have the session opened
            if (_sessions.ContainsKey(id))
            {
                return;
            }

            SafeChromeSession? session = null;
            try
            {
                // connect to the session
                session = new SafeChromeSession(debuggerUrl);

                // find the document
                var document = await session.DOM.GetDocument(new GetDocumentCommand(), token).TimeoutAfter(_timeout);

                // determine if the session is a potential call window
                var isCallSession = await IsCallSession(session, document, token);
                if (isCallSession == null)
                {
                    // faulted - clean up
                    session.Dispose();
                    session = null;
                    return;
                }

                if (isCallSession == false)
                {
                    // save as not interesting
                    _sessions.Add(id, null);

                    // clean up
                    session.Dispose();
                    session = null;
                    return;
                }
                
                // find the mute button
                var muteButton = await session.DOM.QuerySelector(new QuerySelectorCommand
                {
                    NodeId = document.Root.NodeId,
                    Selector = "button#microphone-button",
                }, token).TimeoutAfter(_timeout);

                // only interested in pages with the mute button
                // (but the mute button may be loaded later = do not save so we check again)
                if (muteButton.NodeId == 0)
                {
                    // clean up
                    session.Dispose();
                    session = null;
                    return;
                }
                
                // save this session that we are interested in
                _sessions.Add(id, new ChromeSessionState(session, muteButton.NodeId));
            }
            catch(Exception)
            {
                if (session != null)
                {
                    session.Dispose();
                }
                _sessions.Remove(id);
            }
        }

        /// <summary>
        /// Determine if the session is a for a call.
        /// Return null if faulted.
        /// </summary>
        private static async Task<bool?> IsCallSession(SafeChromeSession session, GetDocumentCommandResponse document, CancellationToken token)
        {
            if (session.IsFaulted)
            {
                return null;
            }

            var body = await session.DOM.QuerySelector(new QuerySelectorCommand
            {
                NodeId = document.Root.NodeId,
                Selector = "body",
            }, token).TimeoutAfter(_timeout);

            var attributes = await session.DOM.GetAttributes(new GetAttributesCommand
            {
                NodeId = body.NodeId,
            }, token).TimeoutAfter(_timeout);

            var attrributePairs = AttributesToDictionary(attributes.Attributes);

            if (attrributePairs.TryGetValue("id", out var id) && string.Equals(id, "child-window-body"))
            {
                // large window
                return true;
            }

            if (attrributePairs.TryGetValue("role", out var role) && string.Equals(role, "presentation"))
            {
                // small window
                return true;
            }

            return false;
        }

        /// <summary>
        /// Check all connected sessions to see if any session has the microphone on.
        /// </summary>
        private async Task CheckSessionsMicrophoneOn(CancellationToken token)
        {
            // temporary state
            var isSessionActive = false;
            var isMicrophoneOn = false;

            // iterate over sessions
            var ids = _sessions.Keys.ToList();
            foreach(var id in ids)
            {
                var session = _sessions[id];
                if (session is null)
                {
                    // not a session with a mute button
                    continue;
                }

                var micOn = await CheckSessionMicrophoneOn(session, token);
                if (micOn == null)
                {
                    // this session had issues, remove it so we validate/recreate it again next time
                    session.Dispose();
                    _sessions.Remove(id);
                    continue;
                }

                // we have at least 1 valid session
                isSessionActive = true;

                // we have at least 1 session with the mic on
                if (micOn == true)
                {
                    // exit early
                    isMicrophoneOn = true;
                    break;
                }
            }

            lock (_stateLock)
            {
                _isSessionActive = isSessionActive;
                _isMicrophoneOn = isMicrophoneOn;
            }
        }

        /// <summary>
        /// Check the specified session to see if the microphone is on.
        /// </summary>
        private static async Task<bool?> CheckSessionMicrophoneOn(ChromeSessionState sessionState, CancellationToken token)
        {
            // clean up faulted sessions
            if (sessionState.Session.IsFaulted)
            {
                return null;
            }

            try
            {
                // get the attributes of the mute button
                var attributes = await sessionState.Session.DOM.GetAttributes(new GetAttributesCommand 
                { 
                    NodeId = sessionState.MuteButtonNodeId,
                }, token).TimeoutAfter(_timeout);
                var attrributePairs = AttributesToDictionary(attributes.Attributes);

                // the minimised call window overlay
                if (attrributePairs.TryGetValue("title", out var title))
                {
                    if (title.Equals("Mute microphone"))
                    {
                        return true;
                    }
                    else if (title.Equals("Unmute"))
                    {
                        return false;
                    }
                }

                // the normal/maximixed call window
                if (attrributePairs.TryGetValue("data-state", out var dataState))
                {
                    if (dataState.Equals("mic"))
                    {
                        return true;
                    }
                    else if (dataState.Equals("mic-off"))
                    {
                        return false;
                    }
                }

                // button not found
                return null;
            } 
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Convert the 1D string array of attribute key/value pairs to a dictionary.
        /// </summary>
        private static Dictionary<string, string> AttributesToDictionary(string[] array)
        {
            var dict = new Dictionary<string, string>();

            for (var i = 0; i < array.Length; i += 2)
            {
                var key = array[i];
                var value = i < array.Length ? array[i + 1] : "";

                dict.Add(key, value);
            }

            return dict;
        }

    }
}
