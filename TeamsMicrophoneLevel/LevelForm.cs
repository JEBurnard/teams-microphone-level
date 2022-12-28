using System.Text;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Graphics.Skia;
using TeamsMicrophoneLevel.Properties;
using Color = Microsoft.Maui.Graphics.Color;
using Font = Microsoft.Maui.Graphics.Font;
using HorizontalAlignment = Microsoft.Maui.Graphics.HorizontalAlignment;
using Point = System.Drawing.Point;

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
        private bool _isStatusConnected = false;
        private bool _isCallActive = false;
        private bool _isMicrophoneOn = false;

        // display text
        private readonly Font _font = new("Consolas");
        private const float _fontSize = 10;
        private const int _margin = 3;

        // mouse events for window movement
        private bool _isMouseDown = false;
        private Point _dragStartLocation = new(0, 0);


        public LevelForm()
        {
            InitializeComponent();

            // restore dragged location
            Location = Settings.Default.Location;

            // restore position if now off screen
            // (eg a screen has been detached)
            if (!IsFullyVisible())
            {
                Location = new Point(0, 0);
                Settings.Default.Location = Location;
                Settings.Default.Save();
            }
        }

        private bool IsFullyVisible()
        {
            var topLeft = Location;
            var bottomRight = new Point(Left + Width, Top + Height);
            foreach (var screen in Screen.AllScreens)
            {
                if (screen.Bounds.Contains(topLeft) && screen.Bounds.Contains(bottomRight))
                {
                    return true;
                }
            }
            return false;
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

        public void OnIsCallStatusConnectedChanged(bool isConnected)
        {
            _isStatusConnected = isConnected;
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

            if (!_isStatusConnected)
            {
                status.Append("No Teams Connection");
            }
            else if (!_isCallActive)
            {
                status.Append("No Call");
            }
            else
            {
                status.Append("In Call");
                if (!_isMicrophoneOn)
                {
                    status.Append(" - Muted");
                }
            }

            return status.ToString();
        }

        private Color GetVolumeColour()
        {
            if (_deviceName == null || !_isStatusConnected)
            {
                // unknown status of teams
                return Colors.DarkGrey;
            }

            if (!_isCallActive)
            {
                // not in a call
                return Colors.Gold;
            }
            
            if (!_isMicrophoneOn)
            {
                // muted
                return Colors.Red;
            }

            // mic on
            return Colors.LightGreen;
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
            using var canvas = new SkiaCanvas 
            {
                Canvas = e.Surface.Canvas, 
                FillColor = Colors.Black, 
            };
            canvas.FillRectangle(0, 0, width, height);

            // draw volume power
            canvas.FillColor = GetVolumeColour();
            canvas.FillRectangle(0, 0, width * volumePercentage, height);

            // draw text
            canvas.Font = _font;
            canvas.FontSize = _fontSize;
            canvas.FontColor = Colors.White;
            var stringSize = canvas.GetStringSize(text, _font, _fontSize);
            var fontHeight = stringSize.Height;
            canvas.DrawString(text, _margin, fontHeight + _margin, HorizontalAlignment.Left);

            // trigger resize (next paint) if the form is to small
            var fontWidth = (int)Math.Ceiling(stringSize.Width);
            if (fontWidth > Width)
            {
                Width = fontWidth;
            }
        }


        private void VolumeControl_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                _isMouseDown = true;
                _dragStartLocation = e.Location;
            }
        }

        private void VolumeControl_MouseUp(object sender, MouseEventArgs e)
        {
            _isMouseDown = false;
        }

        private void VolumeControl_MouseMove(object sender, MouseEventArgs e)
        {
            if (_isMouseDown)
            {
                // dragging, repoisition
                var offsetX = e.X - _dragStartLocation.X;
                var offsetY = e.Y - _dragStartLocation.Y;
                Location = new Point(Left + offsetX, Top + offsetY);


                // save last location
                Settings.Default.Location = Location;
                Settings.Default.Save();
            }
        }
    }
}