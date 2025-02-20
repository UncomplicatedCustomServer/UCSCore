using Exiled.API.Interfaces;
using System.ComponentModel;

namespace UncomplicatedCustomServerCore
{
    public class Translations : ITranslation
    {
        [Description("The warn broadcast message. Available placeholders: %reason%, %warn_count%, %warn_author%")]
        public string WarnBroadcast { get; set; } = "You have been <b><color=red>WARNED</color></b>!\nReason: %reason%";

        [Description("The title of the warn webhook")]
        public string WarningWebhookTitle { get; set; } = "NEW WARN ADDED";
    }
}
