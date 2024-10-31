using Exiled.API.Features;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using UncomplicatedCustomServerCore.Schemas;

namespace UncomplicatedCustomServerCore.NET
{
#pragma warning disable CS4014

    internal class HttpServer
    {
        internal static readonly HttpListener httpListener = new();

        private static readonly List<IPAddress> blacklist = [];

        private static readonly Dictionary<IPAddress, int> failedChallenges = [];

        public HttpServer(string url)
        {
            Log.Info($"Starting HTTP server over {url}");
            httpListener.Prefixes.Add(url);
            httpListener.Start();
            Log.Info($"HTTP server successfully started, awaiting requests...");
            Task.Run(async () =>
            {
                while (true)
                {
                    HttpListenerContext context = await httpListener.GetContextAsync();

                    if (blacklist.Contains(context.Request.RemoteEndPoint.Address))
                        return;

                    HandleRequest(context);
                }
            });
        }

        public void HandleRequest(HttpListenerContext context)
        {
            Log.Info($"New HTTP challenge over {context.Request.RawUrl}\nAuthentication: {context.Request.Headers.Get("Authentication")}");
            if (!Authenticate(context))
                return;

            switch (context.Request.RawUrl)
            {
                case "/ping":
                    Answer(context, "pong");
                    break;
                case "/stats":
                    Answer(context, new GenericStats().Encode(), "application/json");
                    break;
            }
        }

        private bool Authenticate(HttpListenerContext context)
        {
            if (context.Request.RawUrl is "/ping" && Plugin.Instance.Config.AllowUnauthenticatedPing)
            {
                Answer(context, "pong");
                return false;
            }

            if (context.Request.Headers.Get("Authentication") != $"Bearer {Plugin.Instance.Config.PrivateKey}")
            {
                if (failedChallenges.ContainsKey(context.Request.RemoteEndPoint.Address))
                    failedChallenges[context.Request.RemoteEndPoint.Address]++;
                else
                    failedChallenges.Add(context.Request.RemoteEndPoint.Address, 1);

                Answer(context, "Unhautorized", statusCode: 401);

                if (failedChallenges[context.Request.RemoteEndPoint.Address] >= Plugin.Instance.Config.MaxAuthChallenges)
                {
                    failedChallenges.Remove(context.Request.RemoteEndPoint.Address);
                    blacklist.Add(context.Request.RemoteEndPoint.Address);
                }
                return false;
            }
            return true;
        }

        private async Task Answer(HttpListenerContext context, string text, string contentType = "text/plain", int statusCode = 200)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(text);
            context.Response.ContentLength64 = buffer.Length;
            context.Response.ContentType = contentType;
            context.Response.StatusCode = statusCode;

            await context.Response.OutputStream.WriteAsync(buffer, 0, buffer.Length);

            context.Response.Close();
        }
    }
}
