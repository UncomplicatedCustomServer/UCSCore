namespace UncomplicatedCustomServerCore.API.Features
{
    internal class Endpoints
    {
        public static string Warns => $"https://api.ucserver.it/main/ucscore/warns?id={Plugin.Instance.Id}&duration={Plugin.Instance.Config.WarnDuration}";

        public static string WarnWebhooks => $"https://api.ucserver.it/main/webhooks/warn_webhook";

        public static string BanWebhooks => $"https://api.ucserver.it/main/webhooks/ban_webhook";

        public static string Bans => $"https://api.ucserver.it/main/ucscore/bans?id={Plugin.Instance.Id}";
    }
}
