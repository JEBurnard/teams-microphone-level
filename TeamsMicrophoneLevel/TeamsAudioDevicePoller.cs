using NAudio.CoreAudioApi;

namespace TeamsMicrophoneLevel
{
    internal class TeamsAudioDevicePoller
    {
        private string? _currentDevice = null;
        private object _currentDeviceLock = new object();


        /// <summary>
        /// Poll MM microphone devices to find the (first) that is in use by the teams process.
        /// </summary>
        public Task Poll()
        {
            string? deviceName = null;

            // iterate capture (microphone) devices
            var deviceEnumerator = new MMDeviceEnumerator();
            var devices = deviceEnumerator.EnumerateAudioEndPoints(DataFlow.Capture, DeviceState.Active);
            foreach (var device in devices)
            {
                if (deviceName != null)
                {
                    // exit early if the devices used by teams has been already found
                    break;
                }

                var sessionManager = device.AudioSessionManager;
                if (sessionManager == null || sessionManager.Sessions == null)
                {
                    // skip this device if we cannot check sessions
                    continue;
                }

                // iterate sessions using the device
                for (var i = 0; i < sessionManager.Sessions.Count; ++i)
                {
                    var session = sessionManager.Sessions[i];
                    if (session.GetSessionIdentifier.Contains("Teams.exe"))
                    {
                        // this session is for the teams process, save and exit early
                        deviceName = device.DeviceFriendlyName;
                        break;
                    }
                }
            }

            // update current device
            lock(_currentDeviceLock)
            {
                _currentDevice = deviceName;
            }

            return Task.CompletedTask;
        }

        /// <summary>
        /// Get the microphone device name that is in use by the teams process.
        /// Null if none found.
        /// </summary>
        public string? CurrentDevice
        {
            get
            {
                lock (_currentDeviceLock)
                {
                    return _currentDevice;
                }
            }
        }

    }
}
