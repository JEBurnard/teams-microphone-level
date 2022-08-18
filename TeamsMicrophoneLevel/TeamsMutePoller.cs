using BaristaLabs.ChromeDevTools.Runtime;
using BaristaLabs.ChromeDevTools.Runtime.DOM;
using Newtonsoft.Json;

namespace TeamsMicrophoneLevel
{
    internal class TeamsMutePoller
    {
        private readonly int _debugPort;
        private bool _isConnected = false;
        private bool _isSessionActive = false;
        private bool _isMicrophoneOn = false;
        private readonly object _stateLock = new object();
        private readonly Dictionary<string, ChromeSessionState?> _sessions = new Dictionary<string, ChromeSessionState?>();


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

        /// <summary>
        /// Poll the teams electron process (running in debug mode) to see if the mute
        /// button is clicked.
        /// </summary>
        public async Task Poll()
        {
            await ConnectSessions();
            await CheckSessionsMicrophoneOn();
        }

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
        private async Task ConnectSessions()
        {
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
                        await CheckAndAddSession(sessionInfo.Id, sessionInfo.WebSocketDebuggerUrl);
                    }
                }
            }
            catch(Exception)
            {
                _isConnected = false;
                _isSessionActive = false;
                _isMicrophoneOn = false;
            }
        }

        /// <summary>
        /// Add a session if it contains the mute button we want to inspect.
        /// </summary>
        private async Task CheckAndAddSession(string? id, string? debuggerUrl)
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

                // find the mute button
                var document = await session.DOM.GetDocument(new GetDocumentCommand());
                var muteButton = await session.DOM.QuerySelector(new QuerySelectorCommand
                {
                    NodeId = document.Root.NodeId,
                    Selector = "button#microphone-button",
                });

                // only interested in pages with the mute button
                if (muteButton.NodeId == 0)
                {
                    // save as not interesting
                    _sessions.Add(id, null);

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
            }
        }

        /// <summary>
        /// Check all connected sessions to see if any session has the microphone on.
        /// </summary>
        private async Task CheckSessionsMicrophoneOn()
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

                var micOn = await CheckSessionMicrophoneOn(session);
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
        private async Task<bool?> CheckSessionMicrophoneOn(ChromeSessionState sessionState)
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
                });
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
        public static Dictionary<string, string> AttributesToDictionary(string[] array)
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
