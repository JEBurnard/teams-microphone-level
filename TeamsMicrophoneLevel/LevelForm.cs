namespace TeamsMicrophoneLevel
{
    public partial class LevelForm : Form
    {
        public LevelForm()
        {
            InitializeComponent();
        }

        public void OnLevelAvaliable(double power)
        {
            // todo
            Console.WriteLine($"OnLevelAvaliable = {power}");
        }

        public void OnIsCallActiveChanged(bool isCallActive)
        {
            // todo
            Console.WriteLine($"OnIsCallActiveChanged = {isCallActive}");
        }

        public void OnIsMicrophoneOnChanged(bool isMicrophoneOn)
        {
            // todo
            Console.WriteLine($"OnIsMicrophoneOnChanged = {isMicrophoneOn}");
        }
    }
}