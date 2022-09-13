using BaristaLabs.ChromeDevTools.Runtime;
using System.Reflection;
using WebSocket4Net;

namespace TeamsMicrophoneLevel
{
    internal class SafeChromeSession : ChromeSession, IDisposable
    {
        private readonly MethodInfo _receivedMethod;
        private readonly MethodInfo _openedMethod;
        private WebSocket? _socket;

        public SafeChromeSession(string endpointAddress)
            : base(endpointAddress)
        {
            // hack to bypass the error handler that throws (uncaught) exceptions, which crashes us

            // get the private member + handlers we want to keep
            var baseProperty = typeof(ChromeSession).GetField("m_sessionSocket", BindingFlags.Instance | BindingFlags.NonPublic);
            var receivedMethod = typeof(ChromeSession).GetMethod("Ws_MessageReceived", BindingFlags.Instance | BindingFlags.NonPublic);
            var openedMethod = typeof(ChromeSession).GetMethod("Ws_Opened", BindingFlags.Instance | BindingFlags.NonPublic);
            if (baseProperty == null || receivedMethod == null || openedMethod == null)
            {
                throw new Exception("Failed to load required fields and methods");
            }
            _receivedMethod = receivedMethod;
            _openedMethod = openedMethod;

            // create a replacement socket using our handlers instead
            _socket = new WebSocket(endpointAddress)
            {
                EnableAutoSendPing = false
            };
            _socket.MessageReceived += SocketMessageReceived;
            _socket.Error += SocketError;
            _socket.Opened += SocketOpened;

            // remove old socket
            var originalSocket = baseProperty.GetValue(this) as WebSocket;
            if (originalSocket != null)
            {
                originalSocket.Dispose();
            }

            // add new socket
            baseProperty.SetValue(this, _socket);
            IsFaulted = false;
        }

        public new void Dispose()
        {
            if (_socket != null)
            {
                _socket.Dispose();
                _socket = null;
            }
            base.Dispose();
        }

        public bool IsFaulted { get; private set; }

        private void SocketMessageReceived(object? sender, MessageReceivedEventArgs e)
        {
            _receivedMethod.Invoke(this, new[] { sender, e });
        }

        private void SocketError(object? sender, SuperSocket.ClientEngine.ErrorEventArgs e)
        {
            // signal that this session is dead / should be recreated
            IsFaulted = true;
        }

        private void SocketOpened(object? sender, EventArgs e)
        {
            _openedMethod.Invoke(this, new[] { sender, e });
        }
    }
}
