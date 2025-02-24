using Exiled.API.Features;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using UncomplicatedCustomServerCore.Extensions;

namespace UncomplicatedCustomServerCore.API.Features.Warns
{
    public class Warn
    {
        /// <summary>
        /// Gets a list of every warn
        /// </summary>
        [JsonIgnore]
        public static readonly List<Warn> List = [];

        [JsonIgnore]
        public static bool IsEnabled => Plugin.Instance.Config.EnableModerationSystem;

        [JsonProperty("id")]
        public string Id { get; private set; }

        [JsonProperty("username")]
        public string User { get; }

        [JsonProperty("userid")]
        public string UserId { get; }

        [JsonProperty("issuername")]
        public string Issuer { get; }

        [JsonProperty("issuerid")]
        public string IssuerId { get; }

        [JsonProperty("time")]
        public long Time { get; }

        [JsonProperty("reason")]
        public string Reason { get; }

        [JsonIgnore]
        private bool Sync { get; set; } = false;

        [JsonConstructor]
        public Warn(string id, string user, string userId, string issuer, string issuerId, long time, string reason)
        {
            Id = id;
            User = user;
            UserId = userId;
            Issuer = issuer;
            IssuerId = issuerId;
            Time = time;
            Reason = reason;
            Sync = true;

            List.Add(this);
        }

        public Warn(Player user, Player issuer, string reason)
        {
            User = user.Nickname;
            UserId = user.UserId;
            Issuer = issuer.Nickname;
            IssuerId = issuer.UserId;
            Reason = reason;
        }

        public override string ToString()
        {
            TimeSpan diff = DateTimeOffset.FromUnixTimeSeconds(Time).ToLocalTime().AddSeconds(Plugin.Instance.Config.WarnDuration) - DateTimeOffset.Now;
            return $"<size=23><b>#{Id}</b></size>\n<size=18>   <b>User:</b> {User} ({UserId})\n   <b>Issuer:</b> {Issuer} ({IssuerId})\n   <b>Date:</b> {DateTimeOffset.FromUnixTimeSeconds(Time).ToLocalTime():dd/MM/yyyy HH:mm}\n   <b>Remaining:</b> {diff.Hours} hour(s), {diff.Minutes} minute(s)\n   <b>Reason:</b> {Reason}</size>";
        }

        internal async Task<bool> Submit()
        {
            if (Sync)
                return false;

            try
            {
                HttpResponseMessage message = await Plugin.HttpClient.GetAsync($"{Endpoints.Warns}&action=WARN_ADD&userid={UserId}&username={User}&issuername={Issuer}&issuerid={IssuerId}&reason={Reason}");

                if (message.StatusCode is not System.Net.HttpStatusCode.Created)
                    return false;

                Id = await message.Content.ReadAsStringAsync();

                SendPublicWebhook();
                SendStaffWebhook();

                return true;
            }
            catch (Exception e)
            {
                Log.Error(e);
                return false;
            }
        }

        internal void SendPublicWebhook()
        {
            if (Plugin.Instance.Config.WarnWebhook is not null && Plugin.Instance.Config.WarnWebhook.Length > 25 && Plugin.Instance.Config.WarnWebhook.Contains("https://discord.com/api/webhooks/"))
                Plugin.HttpClient.GetAsync($"{Endpoints.WarnWebhooks}?t=public_warn&id={Plugin.Instance.Config.WarnWebhook.Replace("https://discord.com/api/webhooks/", "").Base64Encode()}&warnid={Id}&issuer={IssuerId}&user={UserId}&reason={Reason.Base64Encode()}&warn_number={List.Count(w => w.UserId == UserId) + 1}{BuildTranslations()}");
        }

        internal void SendStaffWebhook()
        {
            if (Plugin.Instance.Config.StaffWarnWebhook is not null && Plugin.Instance.Config.StaffWarnWebhook.Length > 25 && Plugin.Instance.Config.StaffWarnWebhook.Contains("https://discord.com/api/webhooks/"))
                Plugin.HttpClient.GetAsync($"{Endpoints.WarnWebhooks}?t=staff_warn&id={Plugin.Instance.Config.StaffWarnWebhook.Replace("https://discord.com/api/webhooks/", "").Base64Encode()}&warnid={Id}&issuer={IssuerId}&user={UserId}&reason={Reason.Base64Encode()}&warn_number={List.Count(w => w.UserId == UserId) + 1}{BuildTranslations()}");
        }

        internal async Task<bool> Remove(Player issuer, string reason)
        {
            if (!Sync)
                return false;

            try
            {
                HttpResponseMessage message = await Plugin.HttpClient.GetAsync($"{Endpoints.Warns}&action=WARN_REMOVE&warnid={Id}&userid={issuer.UserId}&username={issuer.Nickname}&reason={reason}");

                if (message.StatusCode is System.Net.HttpStatusCode.NoContent)
                    return true;

                Log.Error(message.StatusCode);
                return false;
            }
            catch (Exception e)
            {
                Log.Error(e);
                return false;
            }
        }

        public static Warn Create(Player user, Player issuer, string reason) => new(user, issuer, reason);

        public static async Task<bool> Syncronize()
        {
            List.Clear();

            try
            {
                JsonConvert.DeserializeObject<Warn[]>(await Plugin.HttpClient.GetStringAsync($"{Endpoints.Warns}&action=WARN_LIST"));

                return true;
            }
            catch (Exception e)
            {
                Log.Error(e);

                return false;
            }
        }

        private static string BuildTranslations()
        {
            string result = string.Empty;

            result += $"&0x01={Plugin.Instance.Translation.WarnWebhookMember.Base64Encode()}";
            result += $"&0x02={Plugin.Instance.Translation.WarnWebhookReason.Base64Encode()}";
            result += $"&0x03={Plugin.Instance.Translation.WarnWebhookTitle.Base64Encode()}";
            result += $"&0x04={Plugin.Instance.Translation.WarnWebhookWarnId.Base64Encode()}";

            return result;
        }
    }
}
