using Exiled.Events.EventArgs.Player;
using System;
using System.Linq;
using UncomplicatedCustomServerCore.API.Features.Bans;
using UncomplicatedCustomServerCore.API.Features.Round.Messages;
using UncomplicatedCustomServerCore.API.Utilities;
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

            EventHandler.Verified += OnVerified;
            EventHandler.Left += OnLeft;
        }

        public void OnDisabled()
        {
            if (Plugin.Instance.Config.EnableModerationSystem)
                EventHandler.Banned -= OnBanned;

            if (Plugin.Instance.Config.EnablePlayerStatSystem)
                EventHandler.Died -= OnDied;

            EventHandler.Verified -= OnVerified;
            EventHandler.Left -= OnLeft;
        }

        public void OnVerified(VerifiedEventArgs verified)
        {
            if (ChangeDetector.RefPlayers.Count(p => p.SteamId == verified.Player.UserId) > 0)
                return;

            ChangeDetector.RefPlayers.Add(new(verified.Player));
        }

        public void OnLeft(LeftEventArgs left)
        {
            new PlayerDisconnectMessage(left.Player.UserId).Send();
            ChangeDetector.RefPlayers.RemoveAll(p => p.SteamId == left.Player.UserId);
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
