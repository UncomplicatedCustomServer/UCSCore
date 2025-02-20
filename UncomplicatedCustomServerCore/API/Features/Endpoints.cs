namespace UncomplicatedCustomServerCore.API.Features
{
    internal class Endpoints
    {
        public static string Warns => $"https://api.ucserver.it/main/ucscore/warns?id={Plugin.Instance.Id}&duration={Plugin.Instance.Config.WarnDuration}";

        public static string Webhooks => $"https://api.ucserver.it/main/webhooks/webhook";
    }
}
