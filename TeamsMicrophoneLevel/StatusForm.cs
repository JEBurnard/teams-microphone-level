using System;

namespace TeamsMicrophoneLevel
{
    public partial class StatusForm : Form
    {
        public StatusForm()
        {
            InitializeComponent();
        }

        internal Controller? Controller { get; set; }

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
            var audioDeviceName = Controller?.DevicePoller?.CurrentDeviceName;
            var muteDebugPort = Controller?.MutePoller?.DebugPort;
            var muteIsConnected = Controller?.MutePoller?.IsStatusConnected;
            var muteDebugSessions = Controller?.MutePoller?.Sessions;
            var muteCallSessionActive = Controller?.MutePoller?.IsCallActive;
            var muteMicOn = Controller?.MutePoller?.IsMicrophoneOn;

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
