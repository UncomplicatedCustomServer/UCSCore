using Exiled.Events.EventArgs.Player;
using System;
using UncomplicatedCustomServerCore.API.Features.Bans;

namespace UncomplicatedCustomServerCore.Events
{
    internal class PlayerEvents : ICustomEventHandler
    {
        public void OnEnabled()
        {
            throw new NotImplementedException();
        }

        public void OnDisabled()
        {
            throw new NotImplementedException();
        }

        public async void OnBanned(BannedEventArgs ev)
        {
            await Ban.Create(ev.Target, ev.Player, ev.Details.Reason, int.Parse((DateTimeOffset.Now.ToUnixTimeSeconds() - ev.Details.Expires).ToString())).Submit();
            await Ban.Syncronize();
        }
    }
}
