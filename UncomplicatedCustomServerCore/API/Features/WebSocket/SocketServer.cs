using Exiled.API.Features;
using System.Net;
using WebSocketSharp.Server;

namespace UncomplicatedCustomServerCore.API.Features.WebSocket
{
    internal class SocketServer
    {
        public bool IsEnabled { get; private set; } = false;

        private WebSocketServer Server { get; set; }

        public SocketServer()
        {
            IsEnabled = true;
            Server = new WebSocketServer(IPAddress.Parse("0.0.0.0"), Plugin.Instance.Config.SocketPort);
            Server.AddWebSocketService<ConsoleSocketService>("/console");
            Server.AddWebSocketService<RoundSocketService>("/round");
            Server.Start();
            Log.Info($"Socket server is ready and is listening on 0.0.0.0:{Plugin.Instance.Config.SocketPort}");
        }

        public void Stop()
        {
            Server.Stop();
            IsEnabled = false;
        }
    }
}
