using NAudio.CoreAudioApi;
using NAudio.Wave;

namespace TeamsMicrophoneLevel
{
    internal class LevelStreamer : IDisposable
    {
        // device currently being monitored
        private string? _deviceId = null;

        // device monitor delegate
        private WasapiCapture? _device = null;

        // callback
        private Action<double>? _onLevelAvaliable = null;

        // thread safety
        private readonly object _deviceLock = new();
        private readonly object _callbackLock = new();


        public LevelStreamer()
        { }

        public void Dispose()
        {
            Stop();
        }


        public string? TeamsDeviceId
        {
            get { return _deviceId; }
            set 
            {
                var changed = false;
                lock (_deviceLock)
                {
                    if (!string.Equals(_deviceId, value))
                    {
                        changed = true;
                        _deviceId = value;
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
                lock (_callbackLock)
                {
                    _onLevelAvaliable = value;
                }
            }
        }


        public void Start()
        {
            Stop();
            OpenDevice();
        }

        public void Stop()
        {
            lock (_deviceLock)
            {
                if (_device != null)
                {
                    _device.StopRecording();
                    _device.Dispose();
                    _device = null;
                }
            }
        }

        private void OpenDevice()
        {
            lock (_deviceLock)
            { 
                // need to pick a device first
                if (_deviceId == null)
                {
                    return;
                }

                // find the mm device by id
                using var deviceEnumerator = new MMDeviceEnumerator();
                using var mmDevice = deviceEnumerator.GetDevice(_deviceId);

                // open wave device (defaults to 100ms buffer)
                _device = new WasapiCapture(mmDevice)
                {
                    // set up a 1khz, 16 bit int, mono PCM (amplitude) stream
                    WaveFormat = new WaveFormat(rate: 1000, bits: 16, channels: 1)
                };

                // start streaming
                _device.DataAvailable += Device_DataAvailable;
                _device.StartRecording();
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

            // convert to db
            var amplitude = (double)latestMax / short.MaxValue;
            var power = 10.0 * Math.Log(amplitude);

            // report to delegate
            lock (_callbackLock)
            {
                if (_onLevelAvaliable != null)
                {
                    _onLevelAvaliable.Invoke(power);
                }
            }
        }

    }
}
