using UncomplicatedCustomServerCore.API.Features.WebSocket;

namespace UncomplicatedCustomServerCore.API.Features.Round.Messages
{
    internal abstract class MessageBase
    {
        public abstract RoundActionType Action { get; }

        public void Send()
        {
            if (RoundSocketService.Authed.Count > 0)
                RoundSocketService.Broadcast(this);
        }
    }
}
