using Exiled.Events.EventArgs.Player;
using System;
using UncomplicatedCustomServerCore.API.Features.Bans;
using UncomplicatedCustomServerCore.Extensions;
using EventHandler = Exiled.Events.Handlers.Player;

namespace UncomplicatedCustomServerCore.Events
{
    internal class PlayerEvents : ICustomEventHandler
    {
        public void OnEnabled()
        {
            if (Plugin.Instance.Config.EnableModerationSystem)
                EventHandler.Banned += OnBanned;

            if (Plugin.Instance.Config.EnablePlayerStatSystem)
                EventHandler.Died += OnDied;
        }

        public void OnDisabled()
        {
            if (Plugin.Instance.Config.EnableModerationSystem)
                EventHandler.Banned -= OnBanned;

            if (Plugin.Instance.Config.EnablePlayerStatSystem)
                EventHandler.Died -= OnDied;
        }

        public void OnDied(DiedEventArgs died)
        {
            died.Attacker?.AddKill();
            died.Player.AddDeath();
        }

        public async void OnBanned(BannedEventArgs ev)
        {
            await Ban.Create(ev.Target, ev.Player, ev.Details.Reason, int.Parse((DateTimeOffset.Now.ToUnixTimeSeconds() - ev.Details.Expires).ToString())).Submit();
            await Ban.Syncronize();
        }
    }
}
