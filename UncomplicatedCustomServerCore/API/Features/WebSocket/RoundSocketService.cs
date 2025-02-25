using Exiled.API.Enums;
using Exiled.API.Features;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using UncomplicatedCustomServerCore.API.Features.Round.Messages;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace UncomplicatedCustomServerCore.API.Features.WebSocket
{
    internal class RoundSocketService : WebSocketBehavior
    {
        internal static readonly List<RoundSocketService> Authed = [];

        private bool IsAuthed { get; set; } = false;

        public ZoneType CurrentZone { get; set; } = ZoneType.Entrance;

        public static void Broadcast(MessageBase message)
        {
            try
            {
                foreach (RoundSocketService client in Authed)
                    client.SendAsync(EncodeMessage(message), delegate { });
            }
            catch (Exception e)
            {
                Exiled.API.Features.Log.Error($"Failed to broadcast message: {e}");
            }
        }

        protected override void OnMessage(MessageEventArgs e)
        {
            if (!IsAuthed)
                if (e.Data == Plugin.Instance.Config.PrivateKey)
                {
                    IsAuthed = true;
                    Authed.Add(this);
                    SendAsync(EncodeMessage(MapUpdateMessage.PushAll()), delegate { });
                    SendAsync(EncodeMessage(RoundUpdateMessage.Create()), delegate { });
                    foreach (Player player in Player.List)
                        SendAsync(EncodeMessage(PlayerUpdateMessage.Create(player)), delegate { });
                }
                else
                    Context.WebSocket.Close();
        }

        protected override void OnClose(CloseEventArgs _)
        {
            Authed.Remove(this);
        }

        protected override void OnError(WebSocketSharp.ErrorEventArgs _)
        {
            Authed.Remove(this);
        }

        private static string EncodeMessage(MessageBase message)
        {
            return JsonConvert.SerializeObject(message, new JsonSerializerSettings
            {
                ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new SnakeCaseNamingStrategy()
                },
                Formatting = Formatting.Indented
            });
        }
    }
}
