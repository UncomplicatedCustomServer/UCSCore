using Exiled.API.Features;
using Newtonsoft.Json;
using System.Linq;

namespace UncomplicatedCustomServerCore.API.Features.Round.RemoteMessages
{
    internal class RemoteHealPlayerMessage : RemoteMessageBase
    {
        [JsonProperty("target_id")]
        public string TargetId { get; }

        public RemoteHealPlayerMessage(RemoteActionType action, string targetId) : base(action)
        {
            TargetId = targetId;

            Handle();
        }

        private void Handle()
        {
            if (TargetId is null || TargetId.Length < 1)
                throw new System.Exception("TargetId is null or empty.");

            Player player = Player.List.FirstOrDefault(p => p.UserId == TargetId) ?? throw new System.Exception($"Player is null ({TargetId})");

            player.Health = player.MaxHealth;
        }
    }
}
