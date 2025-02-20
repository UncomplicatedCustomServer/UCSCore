using Exiled.API.Features;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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
        public static readonly List<Warn> List = [];

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
            Plugin.HttpClient.GetAsync($"{Endpoints.Webhooks}?warnid={Id}&issuer={IssuerId}&user={UserId}&reason={Reason.Base64Encode()}&title={Plugin.Instance.Translation.WarningWebhookTitle.Base64Encode()}&t=public_warn&id={Plugin.Instance.Config.WarnWebhook.Replace("https://discord.com/api/webhooks/", "").Base64Encode()}");
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
            } catch (Exception e)
            {
                Log.Error(e);

                return false;
            }
        }
    }
}
