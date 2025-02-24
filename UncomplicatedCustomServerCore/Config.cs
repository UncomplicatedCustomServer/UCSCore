using Exiled.API.Interfaces;
using System.ComponentModel;

namespace UncomplicatedCustomServerCore
{
    public class Config : IConfig
    {
        [Description("Whether or not the plugin is enabled")]
        public bool IsEnabled { get; set; } = true;

        [Description("Whether of not the developer (debug) mode is enabled")]
        public bool Debug { get; set; } = true;

        [Description("The port of the HTTP server that will communicate with the Dashboard")]
        public int Port { get; set; } = 7080;

        [Description("The port of the WebSocket server that will communicate with clients. Needed for the remote console and round info features")]
        public int SocketPort { get; set; } = 9985;

        [Description("The dashboard name. For example, for the dashboard test.serverdash.ucserver.it the Id would be test")]
        public string DashboardId { get; set; } = "";

        [Description("The security key to allow the Dashboard to authenticate itself")]
        public string PrivateKey { get; set; } = string.Empty;

        [Description("The maximum number that a client has to perform with the wrong key in order to get rejected")]
        public int MaxAuthChallenges { get; set; } = 2;

        [Description("Wheter or not the ping system should be accessible also by those who are not authenticated")]
        public bool AllowUnauthenticatedPing { get; set; } = true;

        // MODERATION SYSTEM
        [Description("UCS MODERATION SYSTEM\n\n# Whether or not the moderation system (with ban logs) should be enabled")]
        public bool EnableModerationSystem { get; set; } = true;

        [Description("The duration of a warn in seconds. 86400min are 24 hours")]
        public int WarnDuration { get; set; } = 86400;

        [Description("The duration of the warn broadcast")]
        public ushort WarnBroadcastDuration { get; set; } = 5;

        [Description("The maximum number of a warn before the automatic ban. Set a high number to disable it")]
        public int MaximumWarns { get; set; } = 3;

        [Description("The delay between the warn broadcast and the ban")]
        public int BanDelay { get; set; } = 0;

        [Description("The automatic ban duration")]
        public int BanDuration { get; set; } = 10;

        [Description("The automatic ban reason")]
        public string BanReason { get; set; } = "You exceeded the maximum number of warns";

        [Description("The warn webhook (public one)")]
        public string WarnWebhook { get; set; } = "https://discord.com/api/webhooks/...";

        [Description("The warn webhook (staff one)")]
        public string StaffWarnWebhook { get; set; } = "https://discord.com/api/webhooks/...";

        [Description("The ban webhook (public one)")]
        public string BanWebhook { get; set; } = "https://discord.com/api/webhooks/...";

        [Description("The ban webhook (staff one)")]
        public string StaffBanWebhook { get; set; } = "https://discord.com/api/webhooks/...";

        // PLAYER STATS SYSTEM
        [Description("UCS PLAYER STAT SYSTEM\n\n# Whether or not the player stat system should be enabled")]
        public bool EnablePlayerStatSystem { get; set; } = true;

        [Description("The interval (in minutes) between the stats push to our central servers")]
        public int StatsPushInterval { get; set; } = 1;

        // REMOTE CONSOLE SYSTEM
        [Description("UCS REMOTE CONSOLE SYSTEM\n\n# Wheter or not the remote console system should be enabled")]
        public bool EnableRemoteConsoleSystem { get; set; } = true;

        [Description("UCS REMOTE ROUND VIEW SYSTEM\n\n# Wheter or not the remote round viewer system should be enabled")]
        public bool EnableRemoteRoundViewSystem { get; set; } = true;
    }
}
