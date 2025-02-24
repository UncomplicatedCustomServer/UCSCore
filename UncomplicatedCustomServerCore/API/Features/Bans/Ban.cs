using Exiled.API.Features;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using UncomplicatedCustomServerCore.API.Features.Warns;
using UncomplicatedCustomServerCore.Extensions;

namespace UncomplicatedCustomServerCore.API.Features.Bans
{
    internal class Ban
    {
        /// <summary>
        /// Gets a list of every warn
        /// </summary>
        [JsonIgnore]
        public static readonly List<Ban> List = [];

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

        [JsonProperty("duration")]
        public int Duration { get; }

        [JsonIgnore]
        private bool Sync { get; set; } = false;

        [JsonConstructor]
        public Ban(string id, string user, string userId, string issuer, string issuerId, long time, string reason, int duration)
        {
            Id = id;
            User = user;
            UserId = userId;
            Issuer = issuer;
            IssuerId = issuerId;
            Time = time;
            Reason = reason;
            Duration = duration;
            Sync = true;

            List.Add(this);
        }

        public Ban(Player user, Player issuer, string reason, int duration)
        {
            User = user.Nickname;
            UserId = user.UserId;
            Issuer = issuer.Nickname;
            IssuerId = issuer.UserId;
            Reason = reason;
            Duration = duration;
        }

        public override string ToString()
        {
            TimeSpan diff = DateTimeOffset.FromUnixTimeSeconds(Time).ToLocalTime().AddSeconds(Duration) - DateTimeOffset.Now;
            TimeSpan duration = DateTimeOffset.FromUnixTimeSeconds(Duration).ToLocalTime().AddSeconds(Duration) - DateTimeOffset.FromUnixTimeSeconds(Time).ToLocalTime();
            return $"<size=23><b>#{Id}</b></size>\n<size=18>   <b>User:</b> {User} ({UserId})\n   <b>Issuer:</b> {Issuer} ({IssuerId})\n   <b>Date:</b> {DateTimeOffset.FromUnixTimeSeconds(Time).ToLocalTime():dd/MM/yyyy HH:mm}\n   <b>Duration:</b> {duration.Days} day(s), {duration.Hours} hour(s), {duration.Minutes} minute(s)\n   <b>Remaining:</b> {diff.Hours} hour(s), {diff.Minutes} minute(s)\n   <b>Reason:</b> {Reason}</size>";
        }

        internal async Task<bool> Submit()
        {
            if (Sync)
                return false;

            try
            {
                HttpResponseMessage message = await Plugin.HttpClient.GetAsync($"{Endpoints.Bans}&action=BAN_ADD&userid={UserId}&username={User}&issuername={Issuer}&issuerid={IssuerId}&reason={Reason}&duration={Duration}");

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
            TimeSpan duration = DateTimeOffset.FromUnixTimeSeconds(Duration).ToLocalTime().AddSeconds(Duration) - DateTimeOffset.FromUnixTimeSeconds(Time).ToLocalTime();
            if (Plugin.Instance.Config.BanWebhook is not null && Plugin.Instance.Config.BanWebhook.Length > 25 && Plugin.Instance.Config.BanWebhook.Contains("https://discord.com/api/webhooks/"))
                Plugin.HttpClient.GetAsync($"{Endpoints.BanWebhooks}?t=public_ban&id={Plugin.Instance.Config.BanWebhook.Replace("https://discord.com/api/webhooks/", "")}&banid={Id}&issuer={IssuerId}&user={UserId}&reason={Reason.Base64Encode()}&duration={duration.RenderDate().Base64Encode()}{BuildTranslations()}");
        }

        internal void SendStaffWebhook()
        {
            TimeSpan duration = DateTimeOffset.FromUnixTimeSeconds(Duration).ToLocalTime().AddSeconds(Duration) - DateTimeOffset.FromUnixTimeSeconds(Time).ToLocalTime();
            if (Plugin.Instance.Config.StaffBanWebhook is not null && Plugin.Instance.Config.StaffBanWebhook.Length > 25 && Plugin.Instance.Config.StaffBanWebhook.Contains("https://discord.com/api/webhooks/"))
                Plugin.HttpClient.GetAsync($"{Endpoints.BanWebhooks}?t=staff_ban&id={Plugin.Instance.Config.BanWebhook.Replace("https://discord.com/api/webhooks/", "")}&banid={Id}&issuer={IssuerId}&user={UserId}&reason={Reason.Base64Encode()}&duration={duration.RenderDate().Base64Encode()}{BuildTranslations()}");
        }

#nullable enable
        internal async Task<bool> Remove(Player? issuer)
        {
            if (!Sync)
                return false;

            try
            {
                HttpResponseMessage message = await Plugin.HttpClient.GetAsync($"{Endpoints.Bans}&action=BAN_REMOVE&warnid={Id}&userid={issuer?.UserId ?? "dedicated@server"}&username={issuer?.Nickname ?? "Dedicated Server"}");

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
#nullable disable

        public static Ban Create(Player user, Player issuer, string reason, int duration) => new(user, issuer, reason, duration);

        public static async Task<bool> Syncronize()
        {
            List.Clear();

            try
            {
                JsonConvert.DeserializeObject<Warn[]>(await Plugin.HttpClient.GetStringAsync($"{Endpoints.Bans}&action=BAN_LIST"));

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
            string result = "";

            result += $"&0x01={Plugin.Instance.Translation.BanWebhookMember.Base64Encode()}";
            result += $"&0x02={Plugin.Instance.Translation.BanWebhookReason.Base64Encode()}";
            result += $"&0x03={Plugin.Instance.Translation.BanWebhookDuration.Base64Encode()}";
            result += $"&0x04={Plugin.Instance.Translation.BanWebhookTitle.Base64Encode()}";
            result += $"&0x05={Plugin.Instance.Translation.BanWebhookBanId.Base64Encode()}";

            return result;
        }
    }
}
