using Exiled.API.Features;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Collections.Generic;
using UncomplicatedCustomServerCore.API.Features.Console;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace UncomplicatedCustomServerCore.API.Features.WebSocket
{
    internal class ConsoleSocketService : WebSocketBehavior
    {
        public static readonly List<ConsoleSocketService> Authorized = [];

        private bool IsAuthed { get; set; } = false;

        internal static void Broadcast(LogEntry log)
        {
            foreach (ConsoleSocketService conn in Authorized)
                conn.Send(JsonConvert.SerializeObject(log));
        }

        protected override void OnMessage(MessageEventArgs e)
        {
            Exiled.API.Features.Log.Info("Console socket SEND!");
            if (e.Data is not null && e.Data.Length > 0)
            {
                if (!IsAuthed)
                    if (e.Data == Plugin.Instance.Config.PrivateKey)
                    {
                        IsAuthed = true;
                        Authorized.Add(this);
                        SendAsync(EncodeMessage(LogEntry.List), delegate { });
                    }
                    else
                        Context.WebSocket.Close();
                else
                {
                    if (e.Data == "ping")
                        SendAsync("pong", delegate { });
                    else
                        Server.ExecuteCommand(e.Data, null);
                }
            }
        }

        protected override void OnOpen()
        {
            Exiled.API.Features.Log.Info("Console socket connected!");
        }

        protected override void OnClose(CloseEventArgs e)
        {
            Authorized.Remove(this);
        }

        protected override void OnError(WebSocketSharp.ErrorEventArgs e)
        {
            Authorized.Remove(this);
        }

        private static string EncodeMessage(object message)
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
