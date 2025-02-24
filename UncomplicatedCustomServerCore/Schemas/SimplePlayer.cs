using Exiled.API.Features;
using Newtonsoft.Json;
using PlayerRoles;

namespace UncomplicatedCustomServerCore.Schemas
{
    internal class SimplePlayer(Player player)
    {
        [JsonIgnore]
        internal readonly Player player = player;

        public string Nickname { get; } = player.Nickname;

        public string Displayname { get; } = player.DisplayNickname;

        public bool HasRA { get; } = player.RemoteAdminAccess;

        public string SteamId { get; } = player.UserId;

        public int Id { get; } = player.Id;

        public RoleTypeId Role { get; } = player.Role.Type;
    }
}
