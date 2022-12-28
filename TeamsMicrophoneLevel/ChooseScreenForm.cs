using System;

namespace TeamsMicrophoneLevel
{
    public partial class ChooseScreenForm : Form
    {
        private readonly Form _form;

        public ChooseScreenForm(Form form)
        {
            _form = form;
            InitializeComponent();
            LoadScreenInformation();
        }

        private void LoadScreenInformation()
        {
            var formScreen = Screen.FromControl(_form);
            var allScreens = Screen.AllScreens;

            var formScreenIndex = 0;
            for (var i = 0; i < allScreens.Length; ++i)
            {
                if (allScreens[i].Equals(formScreen))
                {
                    formScreenIndex = i;
                    break;
                }
            }

            screenInput.Minimum = 0;
            screenInput.Maximum = allScreens.Length - 1;
            screenInput.Value = formScreenIndex;
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

        private void ScreenInput_ValueChanged(object sender, EventArgs e)
        {
            var index = (int)screenInput.Value;
            _form.Location = Screen.AllScreens[index].WorkingArea.Location;
        }
    }
}
