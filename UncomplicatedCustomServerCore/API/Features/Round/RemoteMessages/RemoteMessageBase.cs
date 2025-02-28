using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace UncomplicatedCustomServerCore.API.Features.Round.RemoteMessages
{
    internal class RemoteMessageBase(RemoteActionType action)
    {
        [JsonProperty("action")]
        public RemoteActionType Action { get; } = action;

        public static RemoteMessageBase Deserialize(string json)
        {
            JToken data = JToken.Parse(json);

            return data.ToObject<RemoteMessageBase>().Action switch
            {
                RemoteActionType.Teleport => data.ToObject<RemoteTeleportMessage>(),
                RemoteActionType.HealPlayer => data.ToObject<RemoteHealPlayerMessage>(),
                RemoteActionType.KillPlayer => data.ToObject<RemoteKillPlayerMessage>(),
                _ => throw new JsonSerializationException($"Unknown RemoteActionType")
            };
        }
    }
}
