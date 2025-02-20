using CommandSystem;
using Exiled.API.Features;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UncomplicatedCustomServerCore.API.Interfaces;
using UncomplicatedCustomServerCore.Extensions;

namespace UncomplicatedCustomServerCore.Commands.Warn
{
    internal class Remove : IChildCommand
    {
        public string Name => "remove";

        public string Description => "Remove a warn from a player";


        public string RequiredPermission => "ucs.warns.remove";

        public bool Executor(List<string> arguments, ICommandSender sender, out string response)
        {
            if (arguments.Count < 2)
            {
                response = "At least two arguments are needed!\nUsage: warn remove <id> <reason>";
                return false;
            }

            Player issuer = Player.Get(sender);

            string id = arguments[0];

            arguments.RemoveAt(0);

            string reason = string.Join(" ", arguments);

            if (API.Features.Warns.Warn.List.Count(w => w.Id == id) < 1)
            {
                response = $"Failed to remove non-existent warn #{id}";
                return false;
            }

            bool result = Task.Run(() => API.Features.Warns.Warn.List.FirstOrDefault(w => w.Id == id).Remove(issuer, reason).GetAwaiter().GetResult()).ReSync();

            Task.Run(() => API.Features.Warns.Warn.Syncronize().GetAwaiter().GetResult()).ReSync();

            if (result)
                response = $"Warn #{id} has been successfully removed!";
            else
                response = $"Failed to remove the warn from the central servers!";

            return true;
        }
    }
}
