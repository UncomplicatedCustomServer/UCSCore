using Newtonsoft.Json;

namespace UncomplicatedCustomServerCore.API.Features.Round.RemoteMessages
{
    internal class RemoteMessageBase
    {
        [JsonProperty("action")]
        public RemoteActionType Action { get; }
    }
}
