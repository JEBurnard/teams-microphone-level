using System;

namespace TeamsMicrophoneLevel
{
    public partial class StatusForm : Form
    {
        private readonly Controller _controller;

        public StatusForm(Controller controller)
        {
            InitializeComponent();
            _controller = controller;  
        }

        private void StatusForm_Shown(object sender, EventArgs e)
        {
            pollTimer.Start();
        }

        private void StatusForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            pollTimer.Stop();
        }

        private void PollTimer_Tick(object sender, EventArgs e)
        {
            var processDebugPort = TeamsProcessController.GetTeamsDebugPort();
            var audioDeviceName = _controller?.DevicePoller?.CurrentDeviceName;
            var muteDebugPort = _controller?.MutePoller?.DebugPort;
            var muteIsConnected = _controller?.MutePoller?.IsStatusConnected;
            var muteDebugSessions = _controller?.MutePoller?.Sessions;
            var muteCallSessionActive = _controller?.MutePoller?.IsCallActive;
            var muteMicOn = _controller?.MutePoller?.IsMicrophoneOn;

            debugPortValueLabel.Text = processDebugPort == null ? "NA" : $"{processDebugPort}";
            deviceValueLabel.Text = audioDeviceName ?? "NA";
            muteDebugPortValueLabel.Text = muteDebugPort == null ? "NA" : $"{muteDebugPort}";
            connectedValueLabel.Text = muteIsConnected == true ? "Yes" : "No";
            debugSessionsValueLabel.Text = muteDebugSessions == null ? "NA" : $"{muteDebugSessions}";
            callSessionActiveValueLabel.Text = muteCallSessionActive == true ? "Yes" : "No";
            micOnValueLabel.Text = muteMicOn == true ? "Yes" : "No";
        }
    }
}
