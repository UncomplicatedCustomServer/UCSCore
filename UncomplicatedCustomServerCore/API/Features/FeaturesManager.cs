using UncomplicatedCustomServerCore.API.Features.Bans;
using UncomplicatedCustomServerCore.API.Features.PlayerStats;
using UncomplicatedCustomServerCore.API.Features.Warns;
using UncomplicatedCustomServerCore.API.Utilities;

namespace UncomplicatedCustomServerCore.API.Features
{
    internal class FeaturesManager
    {
        public static async void Initialize()
        {
            if (Plugin.Instance.Config.EnableModerationSystem)
            {
                await Warn.Syncronize();
                await Ban.Syncronize();
            }

            if (Plugin.Instance.Config.EnablePlayerStatSystem)
                TimeManager.Start();

            if (Plugin.Instance.Config.EnableRemoteConsoleSystem || Plugin.Instance.Config.EnableRemoteRoundViewSystem)
                Plugin.SocketServer = new();

            if (Plugin.Instance.Config.EnableRemoteRoundViewSystem)
                ChangeDetector.Intialize();

        }

        public static void DisableAll()
        {
            if (Plugin.Instance.Config.EnableRemoteConsoleSystem || Plugin.Instance.Config.EnableRemoteRoundViewSystem)
                Plugin.SocketServer.Stop();
        }
    }
}
