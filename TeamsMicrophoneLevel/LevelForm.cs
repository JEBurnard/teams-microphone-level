using System.Text;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Graphics.Skia;
using Font = Microsoft.Maui.Graphics.Font;
using HorizontalAlignment = Microsoft.Maui.Graphics.HorizontalAlignment;

namespace TeamsMicrophoneLevel
{
    public partial class LevelForm : Form
    {
        // volume display min/max range
        private const double _maxPower = 0.0;
        private const double _minPower = -65.0;
        private const double _powerRange = _maxPower - _minPower;

        // form backing data
        private string? _deviceName = null;
        private double _power = -65.0;
        private bool _isCallActive = false;
        private bool _isMicrophoneOn = false;

        // display text
        private readonly Font _font = new("Consolas");
        private const float _fontSize = 10;
        private const int _margin = 3;


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
            if (_deviceName == null)
            {
                return "No Device";
            }

            var status = new StringBuilder();
            status.Append(_deviceName);

            status.Append(" - ");
            status.Append(_isCallActive ? "In Call" : "No Call");

            if (_isCallActive && !_isMicrophoneOn)
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

            // draw text
            canvas.Font = _font;
            canvas.FontSize = _fontSize;
            canvas.FontColor = Colors.White;
            var fontHeight = canvas.GetStringSize(text, _font, _fontSize).Height;
            canvas.DrawString(text, _margin, fontHeight + _margin, HorizontalAlignment.Left);
        }
    }
}