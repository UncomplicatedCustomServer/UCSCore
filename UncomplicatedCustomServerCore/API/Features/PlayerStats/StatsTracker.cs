using Exiled.API.Features;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace UncomplicatedCustomServerCore.API.Features.PlayerStats
{
    internal class StatsTracker()
    {
        [JsonIgnore]
        public static readonly Dictionary<string, StatsTracker> List = [];

        [JsonProperty("kills")]
        public int Kills { get; set; } = 0;

        [JsonProperty("deaths")]
        public int Deaths { get; set; } = 0;

        public static async Task<bool> Put()
        {
            try
            {
                HttpResponseMessage message = await Plugin.HttpClient.PutAsync(Endpoints.PlayerStats, new StringContent(JsonConvert.SerializeObject(List), Encoding.UTF8, "application/json"));

                if (message.StatusCode is not HttpStatusCode.NoContent)
                {
                    Log.Error($"[STAT_P] - Error code: {message.StatusCode}");
                    return false;
                }

                return true;
            }
            catch (Exception e)
            {
                Log.Error(e);
                return false;
            }
        }
    }
}
