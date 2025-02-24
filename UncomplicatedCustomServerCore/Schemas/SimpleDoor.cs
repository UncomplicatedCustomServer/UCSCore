using Exiled.API.Enums;
using Exiled.API.Features.Doors;
using Newtonsoft.Json;

namespace UncomplicatedCustomServerCore.Schemas
{
    internal class SimpleDoor(Door door)
    {
        [JsonIgnore]
        internal readonly Door door = door;

        public int Identifier { get; } = door.InstanceId;

        public SimpleVector Position { get; } = new(door.Position);

        public SimpleVector Rotation { get; } = SimpleVector.FromQuaternion(door.Rotation);

        public bool IsOpen { get; } = door.IsOpen;

        public DoorType Type { get; } = door.Type;

        public bool IsLocked { get; } = door.IsLocked;

        public bool IsGate { get; } = door.IsGate;

        public bool IsCheckpoint { get; } = door.IsCheckpoint;
    }
}
