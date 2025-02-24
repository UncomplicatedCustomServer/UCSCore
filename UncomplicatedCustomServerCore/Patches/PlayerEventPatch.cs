using Exiled.API.Features;
using Exiled.Events.EventArgs.Interfaces;
using Exiled.Events.Features;
using HarmonyLib;
using UncomplicatedCustomServerCore.API.Features.Round.Messages;

namespace UncomplicatedCustomServerCore.Patches
{
    [HarmonyPatch(typeof(Event<object>), nameof(Event<object>.InvokeSafely))]
    internal class PlayerEventPatch
    {
        public static void Prefix(object arg)
        {
            if (arg is IPlayerEvent pev)
                HandlePlayerEvent(pev, arg);

            if (arg is IDoorEvent dev)
                HandleDoorEvent(dev);

            if (arg is IRoomEvent rev)
                HandleRoomEvent(rev);

        }

        private static void HandlePlayerEvent(IPlayerEvent ev, object arg)
        {
            if (Player.List.Count > 0)
                if (ev.Player is not null)
                    PlayerUpdateMessage.Create(ev.Player).Send();
                else if (arg.GetType().GetProperty("Target") is not null && arg.GetType().GetProperty("Target").GetValue(arg) is not null)
                    PlayerUpdateMessage.Create((Player)arg.GetType().GetProperty("Target").GetValue(arg)).Send();
        }

        private static void HandleDoorEvent(IDoorEvent ev)
        {
            
        }

        private static void HandleRoomEvent(IRoomEvent ev)
        {

        }
    }
}
