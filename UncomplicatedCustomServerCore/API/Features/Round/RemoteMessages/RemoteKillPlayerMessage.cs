using Exiled.API.Features;
using Newtonsoft.Json;
using System.Linq;

namespace UncomplicatedCustomServerCore.API.Features.Round.RemoteMessages
{
    internal class RemoteKillPlayerMessage : RemoteMessageBase
    {
        [JsonProperty("target_id")]
        public string TargetId { get; }

        public RemoteKillPlayerMessage(RemoteActionType action, string targetId) : base(action)
        {
            TargetId = targetId;

            Handle();
        }

        private void Handle()
        {
            if (TargetId is null || TargetId.Length < 1)
                return;

            Player player = Player.List.FirstOrDefault(p => p.UserId == TargetId);

            if (player is null)
                return;

            player.Kill("Killed by remote action.");
        }
    }
}
