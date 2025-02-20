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

        [Description("The dashboard name. For example, for the dashboard test.serverdash.ucserver.it the Id would be test")]
        public string DashboardId { get; set; } = "";

        [Description("The security key to allow the Dashboard to authenticate itself")]
        public string PrivateKey { get; set; } = string.Empty;

        [Description("The maximum number that a client has to perform with the wrong key in order to get rejected")]
        public int MaxAuthChallenges { get; set; } = 2;

        [Description("Wheter or not the ping system should be accessible also by those who are not authenticated")]
        public bool AllowUnauthenticatedPing { get; set; } = true;

        // MODERATION SYSTEM
        [Description("UCS MODERATION SYSTEM\n\n# The duration of a warn in seconds. 1440min is a day")]
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
    }
}
