using Exiled.API.Interfaces;
using System.ComponentModel;

namespace UncomplicatedCustomServerCore
{
    public class Translations : ITranslation
    {
        public string Years { get; set; } = "years";

        public string Year { get; set; } = "year";

        public string Months { get; set; } = "months";

        public string Month { get; set; } = "month";

        public string Days { get; set; } = "days";

        public string Day { get; set; } = "day";

        public string Hours { get; set; } = "hours";

        public string Hour { get; set; } = "hour";

        public string Minutes { get; set; } = "minutes";

        public string Minute { get; set; } = "minute";

        [Description("The warn broadcast message. Available placeholders: %reason%, %warn_count%, %warn_author%")]
        public string WarnBroadcast { get; set; } = "You have been <b><color=red>WARNED</color></b>!\nReason: %reason%";

        public string WarnWebhookTitle { get; set; } = "NEW WARN ADDED";

        public string WarnWebhookMember { get; set; } = "Member";

        public string WarnWebhookReason { get; set; } = "Reason";

        public string WarnWebhookWarnId { get; set; } = "Warn ID";

        public string BanWebhookTitle { get; set; } = "NEW BAN ADDED";

        public string BanWebhookMember { get; set; } = "Member";

        public string BanWebhookReason { get; set; } = "Reason";

        public string BanWebhookDuration { get; set; } = "Duration";

        public string BanWebhookBanId { get; set; } = "Ban ID";
    }
}
