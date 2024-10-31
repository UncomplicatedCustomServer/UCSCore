using HarmonyLib;

namespace UncomplicatedCustomServerCore.Patches
{
    [HarmonyPatch(typeof(ServerConsole), nameof(ServerConsole.ReloadServerName))]
    internal class ServerNamePatch
    {
        private static void Postfix() => ServerConsole._serverName += $"<color=#00000000><size=1>UCS {Plugin.Instance.Version.ToString(3)}</size></color>";
    }
}
