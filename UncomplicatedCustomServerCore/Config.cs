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
    }
}
