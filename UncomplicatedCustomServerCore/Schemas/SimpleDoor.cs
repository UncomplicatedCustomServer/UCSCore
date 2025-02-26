using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Doors;
using Newtonsoft.Json;
using UncomplicatedCustomServerCore.Extensions;

namespace UncomplicatedCustomServerCore.Schemas
{
    internal class SimpleDoor(Door door)
    {
        [JsonIgnore]
        internal readonly Door door = door;

        public string Identifier { get; } = new SimpleVector(door.Position).ToString().Base64Encode();

        public SimpleVector Position { get; } = new(door.Position);

        public SimpleVector Rotation { get; } = SimpleVector.FromQuaternion(door.Rotation);

        public bool IsOpen { get; } = door.IsOpen;

        public DoorType Type { get; } = door.Type;

        public bool IsLocked { get; } = door.IsLocked;

        public bool IsGate { get; } = door.IsGate;

        public bool IsCheckpoint { get; } = door.IsCheckpoint;
    }
}
