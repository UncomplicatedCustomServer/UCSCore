using CommandSystem;
using Exiled.Permissions.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using UncomplicatedCustomServerCore.API.Interfaces;

namespace UncomplicatedCustomServerCore.Commands.Warn
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    internal class CommandParent : ParentCommand
    {
        public CommandParent() => LoadGeneratedCommands();

        public override string Command { get; } = "warn";

        public override string[] Aliases { get; } = [];

        public override string Description { get; } = "Manage the UCS warns";

        public override void LoadGeneratedCommands()
        {
            RegisteredCommands.Add(new List());
            RegisteredCommands.Add(new Add());
            RegisteredCommands.Add(new Remove());
        }

        public List<IChildCommand> RegisteredCommands { get; } = [];

        protected override bool ExecuteParent(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (arguments.Count() == 0)
            {
                // Help page
                response = $"\n>> UncomplicatedCustomServerCore v{Plugin.Instance.Version} <<\nby {Plugin.Instance.Author} -- WARN SYSTEM\n\nAvailable commands:";

                foreach (IChildCommand Command in RegisteredCommands)
                {
                    response += $"\n- {Command.Name}  ->  {Command.Description}  [{Command.RequiredPermission}]";
                }

                return true;
            }
            else
            {
                // Arguments compactor:
                List<string> Arguments = [];

                foreach (string Argument in arguments.Where(arg => arg != arguments.At(0)))
                {
                    Arguments.Add(Argument);
                }

                IChildCommand Command = RegisteredCommands.Where(command => command.Name == arguments.At(0)).FirstOrDefault();

                if (Command is not null)
                    if (sender.CheckPermission(Command.RequiredPermission))
                        return Command.Executor(Arguments, sender, out response);
                    else
                    {
                        response = $"You don't have enough permission(s) to execute that command!\nNeeded: {Command.RequiredPermission}";
                        return false;
                    }
                else
                {
                    response = "Command not found!";
                    return false;
                }
            }
        }
    }
}
