﻿using System;

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
        private Task? _task;

        // settings
        private int _debugPort = 8315;

        // callbacks
        private Action<bool>? _onIsCallActiveChanged;
        private Action<bool>? _onIsMicrophoneOnChanged;

        // polling control
        private readonly object _lock = new object();
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
        }


        public TimeSpan PollInterval { get; set; } = TimeSpan.FromSeconds(1);

        public int TeamsDebugPort
        {
            get { return _debugPort; }
            set
            {
                lock (_lock)
                {
                    _debugPort = value;
                    _mutePoller = new TeamsMutePoller(_debugPort);
                }
            }
        }

        public Action<double>? OnLevelAvaliable
        {
            set
            {
                lock (_lock)
                {
                    _levelStreamer.OnLevelAvaliable = value;
                }
            }
        }

        public Action<bool>? OnIsCallActiveChanged
        {
            set
            {
                lock (_lock)
                {
                    _onIsCallActiveChanged = value;
                }
            }
        }

        public Action<bool>? OnIsMicrophoneOnChanged
        {
            set
            {
                lock (_lock)
                {
                    _onIsMicrophoneOnChanged = value;
                }
            }
        }


        public void Start()
        {
            Stop();
            _stop = false;
            _task = Task.Factory.StartNew(() => Run());
        }

        public void Stop()
        {
            if (_task != null)
            {
                _stop = true;
                _task.Wait();
                _task = null;
            }
        }

        private void Run()
        {
            bool wasInCall = false;
            bool wasMuted = false;

            while(!_stop)
            {
                lock (_lock)
                {
                    // run all polling delegates
                    Task.WaitAll(new[] {
                        _audioDevicePoller.Poll(),
                        _mutePoller.Poll()
                    });
                }

                // update device beming monitored
                _levelStreamer.TeamsDeviceId = _audioDevicePoller.CurrentDeviceId;

                // notify callbacks
                lock (_lock)
                {
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
                        if (_onIsMicrophoneOnChanged != null)
                        {
                            _onIsMicrophoneOnChanged.Invoke(isMuted);
                        }
                    }
                }

                // next poll
                if (!_stop)
                {
                    Thread.Sleep(PollInterval);
                }
            }
        }
    }
}
