using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using System;
using System.Linq;
using System.Threading.Tasks;
using UncomplicatedCustomServerCore.API.Features.Bans;
using UncomplicatedCustomServerCore.API.Features.Overflow;
using UncomplicatedCustomServerCore.API.Features.PlayerStats;
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
            if (Plugin.Instance.Config.EnablePlayerStatSystem)
                EventHandler.Died += OnDied;

            EventHandler.Verified += OnVerified;
            EventHandler.Left += OnLeft;
        }

        public void OnDisabled()
        {
            if (Plugin.Instance.Config.EnablePlayerStatSystem)
                EventHandler.Died -= OnDied;

            EventHandler.Verified -= OnVerified;
            EventHandler.Left -= OnLeft;
        }

        public void OnVerified(VerifiedEventArgs verified)
        {
            if (ChangeDetector.RefPlayers.Count(p => p.SteamId == verified.Player.UserId) > 0)
                return;

            TimeManager.TryAdd(verified.Player);

            ChangeDetector.RefPlayers.Add(new(verified.Player));
        }

        public void OnLeft(LeftEventArgs left)
        {
            new PlayerDisconnectMessage(left.Player.UserId).Send();
            ChangeDetector.RefPlayers.RemoveAll(p => p.SteamId == left.Player.UserId);
            TimeManager.TryRemove(left.Player);
        }

        public void OnDied(DiedEventArgs died)
        {
            died.Attacker?.AddKill();
            died.Player.AddDeath();
        }

        // Invoked by BanEventPatch [Patches]
        public static void OnBanned(ReferenceHub issuer, ReferenceHub player, string reason, long duration)
        {
            if (!Plugin.Instance.Config.EnableModerationSystem)
                return;
               
            if (!Bucket.CanExecute($"_banEvent@{player.authManager.UserId}"))
                return;

            Task.Run(async delegate
            {
                try
                {
                    await Ban.Create(player, issuer, reason, uint.Parse(duration.ToString())).Submit();
                    Log.Info("Ban submitted!");
                    await Ban.Syncronize();
                } 
                catch (Exception e)
                {
                    Log.Error(e);
                }

                Bucket.ChronoRemove($"_banEvent@{player.authManager.UserId}");
            });
        }
    }
}
