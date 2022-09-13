using System;

namespace TeamsMicrophoneLevel
{
    public partial class MainForm : Form
    {
        private readonly Controller _controller = new Controller();
        private readonly LevelForm _levelForm = new LevelForm();

        public MainForm()
        {
            InitializeComponent();

            // hook up delegates
            _controller.OnDeviceChanged = x => _levelForm.OnDeviceChanged(x);
            _controller.OnIsCallActiveChanged = x => _levelForm.OnIsCallActiveChanged(x);
            _controller.OnIsMicrophoneChanged = x => _levelForm.OnIsMicrophoneChanged(x);
            _controller.OnLevelAvaliable = x => _levelForm.OnLevelAvaliable(x);

            // show the level ui form
            _levelForm.Show();

            // start polling / getting volume level
            _controller.Start();
        }

        private void ExitMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void LaunchTeams_OnClick(object sender, EventArgs e)
        {
            // todo: close running instance & reopen with debug port (unless already correctly started)
            // prompt user for debug port to use?
        }
    }
}
