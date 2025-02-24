using Exiled.API.Features;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using Exiled.API.Enums;
using System.Collections.Generic;
using System.Linq;
using PlayerRoles;

namespace UncomplicatedCustomServerCore.Schemas
{
    internal class CompletePlayer(Player player) : SimplePlayer(player)
    {
        public Team Team { get; } = player.Role.Team;

        public ZoneType Zone { get; } = player.Zone;

        public RoomType? Room { get; } = player.CurrentRoom?.Type;

        public ItemType[] Inventory { get; } = player.Inventory?.UserInventory.Items.Values.Select(i => i.ItemTypeId).ToArray() ?? [];

        public ItemType? CurrentItem { get; } = player.CurrentItem?.Type;

        public Dictionary<ItemType, ushort> AmmoTypes { get; } = player.Ammo;

        public SimpleVector Position { get; } = new SimpleVector(player.Position);

        public SimpleVector Rotation { get; } = SimpleVector.FromQuaternion(player.Rotation);

        public float Health { get; } = player.Health;

        public float MaxHealth { get; } = player.MaxHealth;

        public float Ahp { get; } = player.ArtificialHealth;

        public float HumeShield { get; } = player.HumeShield;

        public float Stamina { get; } = player.Stamina;

        public string Encode()
        {
            return JsonConvert.SerializeObject(this, new JsonSerializerSettings
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
