using System;

namespace TeamsMicrophoneLevel
{
    public partial class DebugPortForm : Form
    {
        public DebugPortForm(int defaultPort)
        {
            InitializeComponent();
            portInput.Value = defaultPort;
        }

        private void OkButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        public int Port => (int)portInput.Value;
    }
}
