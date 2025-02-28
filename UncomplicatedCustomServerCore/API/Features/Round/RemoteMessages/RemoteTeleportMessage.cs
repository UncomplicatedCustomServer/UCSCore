using Exiled.API.Features;
using Exiled.API.Features.Pickups;
using Newtonsoft.Json;
using System.Linq;
using UncomplicatedCustomServerCore.Schemas;

namespace UncomplicatedCustomServerCore.API.Features.Round.RemoteMessages
{
    internal class RemoteTeleportMessage : RemoteMessageBase
    {
        [JsonProperty("target")]
        public SimplifiedTargetType Target { get; }

        [JsonProperty("target_id")]
        public string TargetId { get; }

        [JsonProperty("new_position")]
        public SimpleVector NewPosition { get; }

        [JsonConstructor]
        public RemoteTeleportMessage(RemoteActionType action, SimplifiedTargetType target, string targetId, SimpleVector newPosition) : base(action)
        {
            Target = target;
            TargetId = targetId;
            NewPosition = newPosition;

            Handle();
        }

        private void Handle()
        {
            if (Target is SimplifiedTargetType.Player)
            {
                Player player = Player.List.FirstOrDefault(p => p.UserId == TargetId);
                if (player is null)
                    return;

                player.Position = NewPosition.ToVector3();
            }
            else if (Target is SimplifiedTargetType.Pickup)
            {
                Pickup pickup = Pickup.List.FirstOrDefault(p => p.Serial == int.Parse(TargetId));
                if (pickup is null)
                    return;

                pickup.Position = NewPosition.ToVector3();
            }
        }
    }
}
