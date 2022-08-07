using BaristaLabs.ChromeDevTools.Runtime;

namespace TeamsMicrophoneLevel
{
    /// <summary>
    /// Tracked state of a chrome (teams call tab) debug session
    /// </summary>
    internal class ChromeSessionState : IDisposable
    {
        public ChromeSessionState(ChromeSession session, long muteButtonNodeId)
        {
            Session = session;
            MuteButtonNodeId = muteButtonNodeId;
        }

        public ChromeSession Session { get; init; }
        public long MuteButtonNodeId { get; init; }

        public void Dispose()
        {
            Session.Dispose();
        }
    }
}
