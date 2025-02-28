using Newtonsoft.Json;

namespace UncomplicatedCustomServerCore.API.Features.Round.Messages
{
    internal class PlayerDisconnectMessage(string steamid) : MessageBase
    {
        public override RoundActionType Action => RoundActionType.MapUpdate;

        [JsonProperty("steam_id")]
        public string SteamId { get; } = steamid;
    }
}
