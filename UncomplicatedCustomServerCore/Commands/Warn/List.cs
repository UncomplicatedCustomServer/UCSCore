using CommandSystem;
using Exiled.API.Features;
using System.Collections.Generic;
using System.Linq;
using UncomplicatedCustomServerCore.API.Interfaces;

namespace UncomplicatedCustomServerCore.Commands.Warn
{
    internal class List : IChildCommand
    {
        public string Name => "list";

        public string Description => "Show every warn given";

        public string RequiredPermission => "ucs.warns.list";

        public bool Executor(List<string> arguments, ICommandSender sender, out string response)
        {
            response = "Warn list:\n";

            if (arguments.Count < 1)
                response += GenerateInfo(API.Features.Warns.Warn.List);
            else
            {
                string name = string.Empty;
                Player target = Player.Get(arguments[0]);
                if (target is null)
                    name = arguments[0];
                else
                    name = target.UserId;

                if (name.Contains("@steam"))
                    response += GenerateInfo(API.Features.Warns.Warn.List.Where(w => w.UserId == name), name);
                else
                    response += GenerateInfo(API.Features.Warns.Warn.List.Where(w => w.User == name), name);
            }

            return true;
        }

#nullable enable
        private string GenerateInfo(IEnumerable<API.Features.Warns.Warn> warns, string? user = null)
        {
            string message = $"<size=25>{(user == null ? "There are" : $"{user} has")} <b>{warns.Count()}</b> warn(s)</size>";

            foreach (API.Features.Warns.Warn warn in warns)
                message += $"\n\n{warn}";

            return message;
        }
    }
}
