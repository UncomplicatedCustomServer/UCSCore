using CommandSystem;
using Exiled.API.Features;
using MEC;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UncomplicatedCustomServerCore.API.Interfaces;
using UncomplicatedCustomServerCore.Extensions;

namespace UncomplicatedCustomServerCore.Commands.Warn
{
    internal class Add : IChildCommand
    {
        public string Name => "add";

        public string Description => "Add a new warn to a player";


        public string RequiredPermission => "ucs.warns.add";

        public bool Executor(List<string> arguments, ICommandSender sender, out string response)
        {
            if (arguments.Count < 2)
            {
                response = "At least two arguments are needed!\nUsage: warn add <username> <reason>";
                return false;
            }

            Player issuer = Player.Get(sender);
            Player player = Player.Get(arguments[0]);

            arguments.RemoveAt(0);

            string reason = string.Join(" ", arguments);
            bool result = Task.Run(() => API.Features.Warns.Warn.Create(player, issuer, reason).Submit().GetAwaiter().GetResult()).ReSync();

            Task.Run(() => API.Features.Warns.Warn.Syncronize().GetAwaiter().GetResult()).ReSync();

            player.ClearBroadcasts();
            player.Broadcast(Plugin.Instance.Config.WarnBroadcastDuration, Plugin.Instance.Translation.WarnBroadcast.Replace("%reason%", reason).Replace("%warn_count%", API.Features.Warns.Warn.List.Count(w => w.UserId == player.UserId).ToString()).Replace("%warn_author%", issuer.Nickname));

            if (result)
                response = $"Player {player.Nickname} has been successfully warned!\nThis is his <b>{API.Features.Warns.Warn.List.Count(w => w.UserId == player.UserId)}</b> warn!";
            else
                response = $"Failed to push the warn inside the central servers!";

            if (API.Features.Warns.Warn.List.Count(w => w.UserId == player.UserId) >= Plugin.Instance.Config.MaximumWarns)
            {
                Timing.CallDelayed(Plugin.Instance.Config.BanDelay, () =>
                {
                    player.Ban(Plugin.Instance.Config.BanDuration, Plugin.Instance.Config.BanReason);
                });
            }

            return true;
        }
    }
}
