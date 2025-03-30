using Exiled.API.Features;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;

namespace UncomplicatedCustomServerCore.API.Features.PlayerStats
{
    internal static class TimeManager
    {
        public static bool IsEnabled => Plugin.Instance.Config.EnablePlayerStatSystem;

        private static HttpClient HttpClient => Plugin.HttpClient;

        private static readonly Dictionary<string, long> Stopwatch = [];

        internal static void TryAdd(Player player)
        {
            if (!Stopwatch.ContainsKey(player.UserId))
                Stopwatch.Add(player.UserId, DateTimeOffset.Now.ToUnixTimeSeconds());
        }

        public static async void TryRemove(Player player)
        {
            if (player.IsNPC)
                return;

            if (!IsEnabled)
                return;

            if (HttpClient is null)
                throw new NullReferenceException();

            if (!Stopwatch.TryGetValue(player.UserId, out long start))
                return;

            Stopwatch.Remove(player.UserId);

            try
            {
                long time = DateTimeOffset.Now.ToUnixTimeSeconds() - start;
                HttpResponseMessage response = await HttpClient.PutAsync($"{Endpoints.PlayerTime}&player={player.UserId}&time={time}", null);

                if (response.StatusCode is not HttpStatusCode.NoContent)
                    Log.Error($"[STM] - Error code: {response.StatusCode}");
            } 
            catch (Exception e)
            {
                Log.Error(e);
            }
        }
    }
}
