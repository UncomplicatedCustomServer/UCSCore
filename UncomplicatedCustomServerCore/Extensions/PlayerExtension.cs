using Exiled.API.Features;
using UncomplicatedCustomServerCore.API.Features.PlayerStats;

namespace UncomplicatedCustomServerCore.Extensions
{
    internal static class PlayerExtension
    {
        public static void AddKill(this Player player)
        {
            if (StatsTracker.List.TryGetValue(player.UserId, out StatsTracker stats))
                stats.Kills++;
            else
                StatsTracker.List.Add(player.UserId, new StatsTracker { Kills = 1 });
        }

        public static void AddDeath(this Player player)
        {
            if (StatsTracker.List.TryGetValue(player.UserId, out StatsTracker stats))
                stats.Deaths++;
            else
                StatsTracker.List.Add(player.UserId, new StatsTracker { Deaths = 1 });
        }
    }
}
