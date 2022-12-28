using System;

namespace TeamsMicrophoneLevel
{
    public partial class MainForm : Form
    {
        private readonly Controller _controller = new();
        private readonly LevelForm _levelForm = new();
        private StatusForm? _statusForm = null;

        public MainForm()
        {
            InitializeComponent();

            // see if teams is running
            var port = TeamsProcessController.GetTeamsDebugPort();
            if (port != null)
            {
                _controller.TeamsDebugPort = port.Value;
            }

            // hook up delegates
            _controller.OnDeviceChanged = x => _levelForm.OnDeviceChanged(x);
            _controller.OnIsCallStatusConnectedChanged = x => _levelForm.OnIsCallStatusConnectedChanged(x);
            _controller.OnIsCallActiveChanged = x => _levelForm.OnIsCallActiveChanged(x);
            _controller.OnIsMicrophoneChanged = x => _levelForm.OnIsMicrophoneChanged(x);
            _controller.OnLevelAvaliable = x => _levelForm.OnLevelAvaliable(x);

            // show the level ui form
            _levelForm.Show();

            // start polling / getting volume level
            _controller.Start();
        }

        /// <summary>
        /// Dispose - modified from designer generated code
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _controller.Dispose();
                _levelForm.Dispose();
                _statusForm?.Dispose();
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }


        private void ExitMenuItem_Click(object sender, EventArgs e)
        {
            _controller.Stop();
            Close();
        }

        private void LaunchTeams_Click(object sender, EventArgs e)
        {
            var usedPort = TeamsProcessController.StartOrCheckTeams(_controller.TeamsDebugPort);
            _controller.TeamsDebugPort = usedPort;
        }

        private void LaunchTeamsWithPortMenuItem_Click(object sender, EventArgs e)
        {
            var portDialog = new DebugPortForm(_controller.TeamsDebugPort);
            if (portDialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            _controller.TeamsDebugPort = portDialog.Port;
            TeamsProcessController.StartTeams(_controller.TeamsDebugPort);
        }

        private void StatusMenuItem_Click(object sender, EventArgs e)
        {
            if (_statusForm == null || _statusForm.IsDisposed)
            {
                // show status form below the level form
                _statusForm = new StatusForm(_controller)
                {
                    Top = _levelForm.Top + _levelForm.Height + 10,
                    Left = _levelForm.Left + 10
                };
                _statusForm.Show();
            }
        }
    }
}
