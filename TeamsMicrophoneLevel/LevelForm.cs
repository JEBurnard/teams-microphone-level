using System.Text;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Graphics.Skia;
using Color = Microsoft.Maui.Graphics.Color;

namespace TeamsMicrophoneLevel
{
    public partial class LevelForm : Form
    {
        // volume display min/max range
        const double _maxPower = 0.0;
        const double _minPower = -65.0;
        const double _powerRange = _maxPower - _minPower;

        // form backing data
        private string? _deviceName = null;
        double _power = -65.0;
        bool _isCallActive = false;
        bool _isMicrophoneOn = false;

        public LevelForm()
        {
            InitializeComponent();
        }

        public void OnDeviceChanged(string? name)
        {
            _deviceName = name;
            UpdateUi();
        }

        public void OnLevelAvaliable(double power)
        {
            _power = power;
            UpdateUi();
        }

        public void OnIsCallActiveChanged(bool isCallActive)
        {
            _isCallActive = isCallActive;
            UpdateUi();
        }

        public void OnIsMicrophoneChanged(bool isMicrophoneOn)
        {
            _isMicrophoneOn = isMicrophoneOn;
            UpdateUi();
        }

        private void UpdateUi()
        {
            volumeControl.Invalidate();
        }

        private string GetStatusText()
        {
            var status = new StringBuilder();

            if (_deviceName == null)
            {
                status.Append("No Device");
                return status.ToString();
            }
            else
            {
                status.Append(_deviceName);
                status.Append(" - ");
            }

            if (!_isCallActive)
            {
                status.Append("No Call");
                return status.ToString();
            }

            if (!_isMicrophoneOn)
            {
                status.Append(" - Muted");
            }

            return status.ToString();
        }

        private float GetVolumePercentage()
        {
            var rangePower = _power - _minPower;
            var rangePercentage = rangePower / _powerRange;
            return (float)rangePercentage;
        }

        private void VolumeControl_PaintSurface(object sender, SkiaSharp.Views.Desktop.SKPaintGLSurfaceEventArgs e)
        {
            var text = GetStatusText();
            float volumePercentage = GetVolumePercentage();

            float width = volumeControl.Width;
            float height = volumeControl.Height;

            // fill black
            var canvas = new SkiaCanvas
            {
                Canvas = e.Surface.Canvas,
                FillColor = Colors.Black,
            };
            canvas.FillRectangle(0, 0, width, height);

            // draw volume power
            canvas.FillColor = Colors.LightGreen;
            canvas.FillRectangle(0, 0, width * volumePercentage, height);

            // draw text in inverted colours
            // todo
        }
    }
}