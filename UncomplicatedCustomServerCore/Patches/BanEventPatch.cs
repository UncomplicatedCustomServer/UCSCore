using CommandSystem;
using Exiled.API.Features;
using Footprinting;
using HarmonyLib;
using UncomplicatedCustomServerCore.Events;

namespace UncomplicatedCustomServerCore.Patches
{
    [HarmonyPatch(typeof(BanPlayer), nameof(BanPlayer.BanUser), [typeof(Footprint), typeof(ICommandSender), typeof(string), typeof(long)])]
    public class BanEventPatch
    {
        private static void Prefix(Footprint target, ICommandSender issuer, string reason, long duration)
        {
            Log.Info("kjhsfdkjnfsdkljsvfdkljhsfdksrhkjshdkjhsdg");
            if (Player.TryGet(issuer, out Player _i))
                PlayerEvents.OnBanned(_i.ReferenceHub, target.Hub, reason, duration);
            else
                Log.Info("[] NO PLAYER FOUND");
        }
    }
}
