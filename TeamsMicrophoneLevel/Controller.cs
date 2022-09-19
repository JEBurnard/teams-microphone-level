using System;

namespace TeamsMicrophoneLevel
{
    /// <summary>
    /// The main logic controller for this system.
    /// </summary>
    internal class Controller : IDisposable
    {
        // delegates
        private TeamsAudioDevicePoller _audioDevicePoller;
        private TeamsMutePoller _mutePoller;
        private LevelStreamer _levelStreamer;
        private Task? _deviceTask;
        private Task? _muteTask;

        // settings
        private int _debugPort = 8315;

        // callbacks
        private Action<string?>? _onDeviceChanged;
        private Action<bool>? _onIsCallStatusConnectedChanged;
        private Action<bool>? _onIsCallActiveChanged;
        private Action<bool>? _onIsMicrophoneChanged;

        // polling control
        private readonly object _lockDevice = new();
        private readonly object _lockMute = new();
        private bool _stop = false;


        public Controller()
        {
            _audioDevicePoller = new TeamsAudioDevicePoller();
            _mutePoller = new TeamsMutePoller(_debugPort);
            _levelStreamer = new LevelStreamer();
        }

        public void Dispose()
        {
            Stop();
            _mutePoller.Dispose();
            _levelStreamer.Dispose();
        }


        public TimeSpan PollInterval { get; set; } = TimeSpan.FromMilliseconds(250);

        public int TeamsDebugPort
        {
            get { return _debugPort; }
            set
            {
                lock (_lockMute)
                {
                    if (_debugPort != value)
                    {
                        _debugPort = value;
                        _mutePoller = new TeamsMutePoller(_debugPort);
                    }
                }
            }
        }

        public Action<string?>? OnDeviceChanged
        {
            set
            {
                lock (_lockDevice)
                {
                    _onDeviceChanged = value;
                }
            }
        }

        public Action<double>? OnLevelAvaliable
        {
            set
            {
                lock (_lockDevice)
                {
                    _levelStreamer.OnLevelAvaliable = value;
                }
            }
        }

        public Action<bool>? OnIsCallStatusConnectedChanged
        {
            set
            {
                lock (_lockMute)
                {
                    _onIsCallStatusConnectedChanged = value;
                }
            }
        }


        public Action<bool>? OnIsCallActiveChanged
        {
            set
            {
                lock (_lockMute)
                {
                    _onIsCallActiveChanged = value;
                }
            }
        }

        public Action<bool>? OnIsMicrophoneChanged
        {
            set
            {
                lock (_lockMute)
                {
                    _onIsMicrophoneChanged = value;
                }
            }
        }


        public void Start()
        {
            Stop();
            _stop = false;
            _deviceTask = Task.Factory.StartNew(() => RunDevicePoll());
            _muteTask = Task.Factory.StartNew(() => RunMutePoll());
        }

        public void Stop()
        {
            _stop = true;
            if (_deviceTask != null)
            {
                _deviceTask.Wait();
                _deviceTask.Dispose();
                _deviceTask = null;
            }
            if (_muteTask != null)
            {
                _muteTask.Wait();
                _muteTask.Dispose();
                _muteTask = null;
            }
        }

        private void RunDevicePoll()
        {
            var lastPoll = DateTime.UtcNow;
            string? oldDeviceName = null;

            while (!_stop)
            {
                lock (_lockDevice)
                {
                    _audioDevicePoller.Poll().Wait();

                    _levelStreamer.TeamsDeviceId = _audioDevicePoller.CurrentDeviceId;

                    var deviceName = _audioDevicePoller.CurrentDeviceName;
                    if (!string.Equals(oldDeviceName, deviceName, StringComparison.InvariantCultureIgnoreCase))
                    {
                        oldDeviceName = deviceName;
                        if (_onDeviceChanged != null)
                        {
                            _onDeviceChanged.Invoke(deviceName);
                        }
                    }
                }

                if (!_stop)
                {
                    DoSleep(lastPoll);
                }
            }
        }

        private void RunMutePoll()
        {
            var lastPoll = DateTime.UtcNow;
            bool wasConnected = false;
            bool wasInCall = false;
            bool wasMuted = false;

            while(!_stop)
            {
                lock (_lockMute)
                {
                    _mutePoller.Poll().Wait();

                    var isConnected = _mutePoller.IsStatusConnected;
                    if (isConnected != wasConnected)
                    {
                        wasConnected = isConnected;
                        if (_onIsCallStatusConnectedChanged != null)
                        {
                            _onIsCallStatusConnectedChanged(isConnected);
                        }
                    }

                    var isInCall = _mutePoller.IsCallActive;
                    if (isInCall != wasInCall)
                    {
                        wasInCall = isInCall;
                        if (_onIsCallActiveChanged != null)
                        {
                            _onIsCallActiveChanged.Invoke(isInCall);
                        }
                    }

                    var isMuted = _mutePoller.IsMicrophoneOn;
                    if (isMuted != wasMuted)
                    {
                        wasMuted = isMuted;
                        if (_onIsMicrophoneChanged != null)
                        {
                            _onIsMicrophoneChanged.Invoke(isMuted);
                        }
                    }
                }

                if (!_stop)
                {
                    DoSleep(lastPoll);
                }
            }
        }

        private DateTime DoSleep(DateTime lastPoll)
        {
            var now = DateTime.UtcNow;
            var msSinceLast = (int)now.Subtract(lastPoll).TotalMilliseconds;
            var msInterval = (int)PollInterval.TotalMilliseconds;
            Thread.Sleep(Math.Max(0, Math.Min(msInterval, msSinceLast)));
            return now;
        }
    }
}
