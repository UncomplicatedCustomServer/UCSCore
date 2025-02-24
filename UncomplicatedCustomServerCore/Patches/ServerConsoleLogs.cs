using HarmonyLib;
using System;
using UncomplicatedCustomServerCore.API.Features.Console;

namespace UncomplicatedCustomServerCore.Patches
{
#pragma warning disable IDE0060 // Remove unused parameter

    [HarmonyPatch(typeof(ServerConsole), nameof(ServerConsole.AddLog))]
    internal class ServerConsoleLogs
    {
        private static void Prefix(string q, ConsoleColor color = ConsoleColor.Gray, bool hideFromOutputs = false)
        {
            new LogEntry(DateTimeOffset.Now, q, color);
        }
    }
}
