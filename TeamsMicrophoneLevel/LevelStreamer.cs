using NAudio.Wave;

namespace TeamsMicrophoneLevel
{
    internal class LevelStreamer : IDisposable
    {
        // device currently being monitored
        private string? _deviceName = null;

        // device monitor delegate
        private WaveInEvent? _device = null;

        // callback
        private Action<double>? _onLevelAvaliable = null;

        // thread safety
        private readonly object _lock = new object();


        public LevelStreamer()
        { }

        public void Dispose()
        {
            Stop();
        }


        public string? TeamsDeviceName
        {
            get { return _deviceName; }
            set 
            {
                var changed = false;
                lock (_lock)
                {
                    if (!string.Equals(_deviceName, value))
                    {
                        changed = true;
                        _deviceName = value;
                    }
                }
                if (changed)
                {
                    Start();
                }
            }
        }

        public bool WasTeamsDeviceFound => _device != null;

        public Action<double>? OnLevelAvaliable
        {
            set
            {
                lock (_lock)
                {
                    _onLevelAvaliable = value;
                }
            }
        }


        public void Start()
        {
            Stop();

            var deviceNumber = GetTeamsDeviceNumber();
            if (deviceNumber == null)
            {
                return;
            }

            // todo: pick better values here
            // eg 44.1khz, 100ms buffer?

            // set up a 1khz, 16 bit int, mono PCM (amplitude) stream
            _device = new WaveInEvent
            {
                DeviceNumber = deviceNumber.Value,
                WaveFormat = new WaveFormat(rate: 1000, bits: 16, channels: 1),
                BufferMilliseconds = 10,
            };
            _device.DataAvailable += Device_DataAvailable;
            _device.StartRecording();
        }

        public void Stop()
        {
            if (_device != null)
            {
                _device.StopRecording();
                _device = null;
            }
        }

        private int? GetTeamsDeviceNumber()
        {
            lock (_lock)
            { 
                // need to pick a device first
                if (_deviceName == null)
                {
                    return null;
                }

                // iterate over wave devices to find the match
                int waveInDevices = WaveIn.DeviceCount;
                for (int waveInDevice = 0; waveInDevice < waveInDevices; waveInDevice++)
                {
                    // wave device name is shorter than the multimedia device name
                    // eg MM FriendlyName = "SteelSeries Sonar - Microphone (SteelSeries Sonar Virtual Audio Device)"
                    //   wave ProductName = "SteelSeries Sonar - Microphone " - limited to 32 chars?
                    var deviceInfo = WaveIn.GetCapabilities(waveInDevice);
                    if (_deviceName.StartsWith(deviceInfo.ProductName))
                    {
                        // todo: does not match
                        // "SteelSeries Sonar Virtual Audio Device" vs "SteelSeries Sonar - Microphone " or "Microphone (7- Arctis 7+)"
                        return waveInDevice;
                    }
                }

                // not found 
                return null;
            }
        }

        private void Device_DataAvailable(object? sender, WaveInEventArgs e)
        {
            // find the largest amplitude in this set of sample
            int latestMax = int.MinValue;
            for (int index = 0; index < e.BytesRecorded; index += 2)
            {
                int value = BitConverter.ToInt16(e.Buffer, index);
                latestMax = Math.Max(latestMax, value);
            }

            // todo: smoothing / windowing / rolling average?
            // rms to average instead of single max level?

            // convert to db
            var amplitude = (double)latestMax / short.MaxValue;
            var power = 10.0 * Math.Log(amplitude);

            // report to delegate
            lock (_lock)
            {
                if (_onLevelAvaliable != null)
                {
                    _onLevelAvaliable.Invoke(power);
                }
            }
        }

    }
}
