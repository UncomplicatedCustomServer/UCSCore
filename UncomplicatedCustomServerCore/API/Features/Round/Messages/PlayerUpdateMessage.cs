using Exiled.API.Features;
using UncomplicatedCustomServerCore.Schemas;

namespace UncomplicatedCustomServerCore.API.Features.Round.Messages
{
    internal class PlayerUpdateMessage(SerializedPlayer player) : MessageBase
    {
        public override RoundActionType Action => RoundActionType.PlayerUpdate;

        public SerializedPlayer Player = player;

        public static PlayerUpdateMessage Create(Player player) => new(new(player));
    }
}
