using Exiled.API.Features;
using Exiled.Loader;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Collections.Generic;
using System.Linq;

namespace UncomplicatedCustomServerCore.Schemas
{
    internal class GenericStats
    {
        public int MaxPlayers { get; }

        public int Players { get; }

        public List<SimplePlayer> PlayerList { get; }

        public string ServerName { get; }

        public bool Verified { get; }

        public List<int> Plugins { get; }

        public GenericStats()
        {
            MaxPlayers = Server.MaxPlayerCount;
            Players = Server.PlayerCount;

            PlayerList = [];
            foreach (Player player in Player.List)
                PlayerList.Add(new(player));

            ServerName = ServerConsole._serverName;
            Verified = Server.IsVerified;

            Plugins = [];
            if (Loader.Plugins.Any(p => p.Name is "UncomplicatedCustomRoles"))
                Plugins.Add(0);

            if (Loader.Plugins.Any(p => p.Name is "UncomplicatedCustomItems"))
                Plugins.Add(1);

            if (Loader.Plugins.Any(p => p.Name is "UncomplicatedDiscordIntegration"))
                Plugins.Add(2);

            if (Loader.Plugins.Any(p => p.Name is "UncomplicatedCustomTeams"))
                Plugins.Add(3);
        }

        public string Encode()
        {
            DefaultContractResolver contractResolver = new()
            {
                NamingStrategy = new SnakeCaseNamingStrategy()
            };

            return JsonConvert.SerializeObject(this, new JsonSerializerSettings
            {
                ContractResolver = contractResolver,
                Formatting = Formatting.Indented
            });
        }
    }
}
