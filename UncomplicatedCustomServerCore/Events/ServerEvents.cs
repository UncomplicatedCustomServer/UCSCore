using Exiled.Events.EventArgs.Server;
using System.Linq;
using System.Threading.Tasks;
using UncomplicatedCustomServerCore.API.Features.Bans;
using UncomplicatedCustomServerCore.API.Features.PlayerStats;
using UncomplicatedCustomServerCore.Extensions;
using ServerHandler = Exiled.Events.Handlers.Server;

namespace UncomplicatedCustomServerCore.Events
{
    internal class ServerEvents : ICustomEventHandler
    {
        public void OnEnabled()
        {
            ServerHandler.RoundStarted += OnRoundStarted;

            if (Plugin.Instance.Config.EnableModerationSystem)
                ServerHandler.Unbanned += OnUnban;

            if (Plugin.Instance.Config.EnablePlayerStatSystem)
                ServerHandler.RoundEnded += OnRoundEnded;
        }

        public void OnDisabled()
        {
            ServerHandler.RoundStarted -= OnRoundStarted;

            if (Plugin.Instance.Config.EnableModerationSystem)
                ServerHandler.Unbanned -= OnUnban;

            if (Plugin.Instance.Config.EnablePlayerStatSystem)
                ServerHandler.RoundEnded -= OnRoundEnded;
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

        public async void OnRoundEnded(RoundEndedEventArgs _)
        {
            await StatsTracker.Put();
            StatsTracker.List.Clear();
        }

        public void OnRoundStarted()
        {
            StatsTracker.List.Clear();
        }
    }
}
