using Exiled.API.Features.Pickups;
using Newtonsoft.Json;

namespace UncomplicatedCustomServerCore.Schemas
{
    internal class SimplePickup(Pickup pickup)
    {
        [JsonIgnore]
        internal readonly Pickup pickup = pickup;

        public ItemType Type { get; } = pickup.Type;

        public int Serial { get; } = pickup.Serial;

        public string Room { get; } = pickup.Room.Identifier.name;

        public SimpleVector Position { get; } = new(pickup.Position);

        public SimpleVector Rotation { get; } = SimpleVector.FromQuaternion(pickup.Rotation);

        public bool IsLocked { get; } = pickup.IsLocked;

        public bool IsSpawned { get; } = pickup.IsSpawned;
    }
}
