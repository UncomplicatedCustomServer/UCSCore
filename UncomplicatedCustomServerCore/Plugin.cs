using Exiled.API.Enums;
using Exiled.API.Features;
using HarmonyLib;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using UncomplicatedCustomServerCore.API.Features;
using UncomplicatedCustomServerCore.NET;

namespace UncomplicatedCustomServerCore
{
    public class Plugin : Plugin<Config, Translations>
    {
        public override string Name => "UncomplicatedCustomServer Core";

        public override string Prefix => "ucscore";

        public override string Author => "FoxWorn3365 & UncomplicatedCustomServer Collective";

        public override Version Version => new(1, 0, 0);

        public override Version RequiredExiledVersion => new(8, 11, 0);

        public override PluginPriority Priority => PluginPriority.Low;

        internal static Plugin Instance { get; private set; }

        internal static HttpClient HttpClient { get; } = new();

        internal static HttpServer HttpServer { get; private set; }

        private Client Client;

        private Harmony _harmony;

        internal string Id { get; private set; }

        public override void OnEnabled()
        {
            Instance = this;

            LoadOrCreateId();

            if (Config.PrivateKey.Length < 10)
            {
                Log.Error("The private_key lenght can't be below 10 characters!\nAborting plugin execution...");
                OnDisabled();
                return;
            }

            if (Config.DashboardId.Length < 3)
            {
                Log.Error("The dashboard_id is not valid!\nAborting plugin execution...");
                OnDisabled();
                return;
            }

            _harmony = new($"ucs.ucscore-{DateTime.Now.Ticks}");
            _harmony.PatchAll();

            Client = new(Config.DashboardId);

            Task.Run(() => HttpServer = new($"http://*:{Config.Port}/"));

            Task.Run(async delegate
            {
                await Task.Delay(5000);
                await Client.SyncPutCustomRoles();
            });

            base.OnEnabled();

            FeaturesManager.Initialize();
        }

        public override void OnDisabled()
        {
            Instance = null;
            
            _harmony.UnpatchAll();
            _harmony = null;

            base.OnDisabled();
        }

        public void LoadOrCreateId()
        {
            if (File.Exists(Path.Combine(Paths.Configs, $".ucscore_{Server.Port}")))
                Id = File.ReadAllText(Path.Combine(Paths.Configs, $".ucscore_{Server.Port}"));
            else
            {
                Id = Guid.NewGuid().ToString();
                File.WriteAllText(Path.Combine(Paths.Configs, $".ucscore_{Server.Port}"), Id);
            }
        }
    }
}
