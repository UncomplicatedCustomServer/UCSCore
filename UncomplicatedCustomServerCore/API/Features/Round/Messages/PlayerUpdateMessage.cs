using Exiled.API.Features;
using UncomplicatedCustomServerCore.Schemas;

namespace UncomplicatedCustomServerCore.API.Features.Round.Messages
{
    internal class PlayerUpdateMessage(CompletePlayer player) : MessageBase
    {
        public override RoundActionType Action => RoundActionType.PlayerUpdate;

        public CompletePlayer Player = player;

        public static PlayerUpdateMessage Create(Player player) => new(new SerializedPlayer(player));
    }
}
