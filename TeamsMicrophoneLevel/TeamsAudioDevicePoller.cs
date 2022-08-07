using NAudio.CoreAudioApi;

namespace TeamsMicrophoneLevel
{
    internal class TeamsAudioDevicePoller
    {
        private string? _currentDeviceId = null;
        private object _currentDeviceLock = new object();


        /// <summary>
        /// Poll MM microphone devices to find the (first) that is in use by the teams process.
        /// </summary>
        public Task Poll()
        {
            string? deviceId = null;

            // iterate capture (microphone) devices
            var deviceEnumerator = new MMDeviceEnumerator();
            var devices = deviceEnumerator.EnumerateAudioEndPoints(DataFlow.Capture, DeviceState.Active);
            foreach (var device in devices)
            {
                if (deviceId != null)
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
                        deviceId = device.ID;
                        break;
                    }
                }
            }

            // update current device
            lock(_currentDeviceLock)
            {
                _currentDeviceId = deviceId;
            }

            return Task.CompletedTask;
        }

        /// <summary>
        /// Get the MMDevice ID of the microphone that is in use by the teams process.
        /// Null if none found.
        /// </summary>
        public string? CurrentDeviceId
        {
            get
            {
                lock (_currentDeviceLock)
                {
                    return _currentDeviceId;
                }
            }
        }

    }
}
