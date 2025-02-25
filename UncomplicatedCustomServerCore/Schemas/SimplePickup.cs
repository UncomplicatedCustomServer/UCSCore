using Exiled.API.Features.Pickups;

namespace UncomplicatedCustomServerCore.Schemas
{
    internal class SimplePickup(Pickup pickup)
    {
        public ItemType Type { get; } = pickup.Type;

        public int Serial { get; } = pickup.Serial;

        public string Room { get; } = pickup.Room.Identifier.name;

        public SimpleVector Position { get; } = new(pickup.Position);

        public SimpleVector Rotation { get; } = SimpleVector.FromQuaternion(pickup.Rotation);

        public bool IsLocked { get; } = pickup.IsLocked;

        public bool IsSpawned { get; } = pickup.IsSpawned;
    }
}
