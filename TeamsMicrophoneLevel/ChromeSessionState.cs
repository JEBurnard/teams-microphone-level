using System;

namespace TeamsMicrophoneLevel
{
    /// <summary>
    /// Tracked state of a chrome (teams call tab) debug session
    /// </summary>
    internal class ChromeSessionState : IDisposable
    {
        public ChromeSessionState(SafeChromeSession session, long muteButtonNodeId)
        {
            Session = session;
            MuteButtonNodeId = muteButtonNodeId;
        }

        public SafeChromeSession Session { get; init; }
        public long MuteButtonNodeId { get; init; }

        public void Dispose()
        {
            Session.Dispose();
        }
    }
}
