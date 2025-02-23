using Exiled.Events.EventArgs.Server;
using System.Linq;
using System.Threading.Tasks;
using UncomplicatedCustomServerCore.API.Features.Bans;
using UncomplicatedCustomServerCore.Extensions;
using ServerHandler = Exiled.Events.Handlers.Server;

namespace UncomplicatedCustomServerCore.Events
{
    internal class ServerEvents : ICustomEventHandler
    {
        public void OnEnabled()
        {
            ServerHandler.Unbanned += OnUnban;
        }

        public void OnDisabled()
        {
            ServerHandler.Unbanned -= OnUnban;
        }

        public async void OnUnban(UnbannedEventArgs ev)
        {
            Ban ban = Ban.List.FirstOrDefault(b => b.UserId == ev.TargetId);

            if (ban is not null)
            {
                await ban.Remove(null);
                await Ban.Syncronize();
            }
        }
    }
}
