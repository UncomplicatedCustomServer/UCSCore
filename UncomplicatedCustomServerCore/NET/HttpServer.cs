using Exiled.API.Features;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using UncomplicatedCustomServerCore.Extensions;
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

            if (context.Request.RawUrl == "/ping")
                Answer(context, "pong");
            else if (context.Request.RawUrl == "/stats")
                Answer(context, new GenericStats().Encode(), "application/json");
            else
                Handle(context, context.Request.RawUrl);
        }

        public void Stop() => httpListener.Stop();

        private bool Authenticate(HttpListenerContext context)
        {
            if (context.Request.RawUrl is "/ping" && Plugin.Instance.Config.AllowUnauthenticatedPing)
            {
                Answer(context, "pong");
                return false;
            }

            if (context.Request.Headers.Get("Authentication") != $"Bearer {Plugin.Instance.Config.PrivateKey}")
            {
                try
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
                } 
                catch (Exception e)
                {
                    Log.Error(e);
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

        private void Handle(HttpListenerContext context, string rawUrl)
        {
            List<string> url = [.. rawUrl.Split('/')];
            url.RemoveAt(0);

            Log.Info(string.Join("/", url));

            if (url[0] == "logs")
                HandleLogEndpoint(context, url);
            else if (url[0] == "stats")
                Answer(context, new GenericStats().Encode(), "application/json");
        }

        private void HandleLogEndpoint(HttpListenerContext context, List<string> url)
        {
            if (url.Count < 2)
            {
                Answer(context, "", statusCode: 400);
            }

            string path = Path.Combine(Paths.AppData, "SCP Secret Laboratory", "LocalAdminLogs", Server.Port.ToString());

            try
            {
                if (url[1] == "path")
                    Answer(context, path);
                else if (url[1] == "list")
                    Answer(context, string.Join(Environment.NewLine, Directory.GetFiles(path).Select(p => p.Replace($"{path}/", ""))));
                else if (File.Exists(Path.Combine(path, url[1].Base64Decode())))
                    Answer(context, ReadAndFixLogFile(Path.Combine(path, url[1].Base64Decode())));
                else
                    Answer(context, "", statusCode: 404);
            }
            catch (Exception e)
            {
                Log.Error($"Error while handling log endpoint: {e}");
                Answer(context, "", statusCode: 500);
            }
        }

        private string ReadAndFixLogFile(string path) => string.Join(Environment.NewLine, File.ReadAllLines(path).Where(l => !l.Contains(" [STDOUT] ")));
    }
}
