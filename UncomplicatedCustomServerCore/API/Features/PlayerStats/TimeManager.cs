using Exiled.API.Features;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace UncomplicatedCustomServerCore.API.Features.PlayerStats
{
    internal static class TimeManager
    {
        public static bool IsEnabled => Plugin.Instance.Config.EnablePlayerStatSystem;

        public static int Interval => Plugin.Instance.Config.StatsPushInterval;

        internal static bool IsAllowed { get; set; } = true;

        private static HttpClient HttpClient => Plugin.HttpClient;

        public static async void Start()
        {
            while (IsEnabled && IsAllowed)
            {
                if (Player.List.Count > 0)
                    await Request();

                await Task.Delay(Interval * 1000 * 60);
            }
        }

        public static void Stop() => IsAllowed = false;

        private static async Task<bool> Request()
        {
            if (HttpClient is null)
                throw new NullReferenceException();

            try
            {
                HttpResponseMessage response = await HttpClient.PostAsync($"{Endpoints.PlayerTime}&duration={Interval}", new StringContent(BuildPlayers(), Encoding.UTF8));

                if (response.StatusCode is not HttpStatusCode.NoContent)
                {
                    Log.Error($"[STM] - Error code: {response.StatusCode}");
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

        private static string BuildPlayers()
        {
            List<string> result = [];

            foreach (Player player in Player.List)
                result.Add(player.UserId);

            return string.Join(Environment.NewLine, result);
        }
    }
}
