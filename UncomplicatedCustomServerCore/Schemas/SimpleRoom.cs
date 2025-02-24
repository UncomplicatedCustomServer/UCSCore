using Exiled.API.Features;
using MapGeneration;
using Newtonsoft.Json;

namespace UncomplicatedCustomServerCore.Schemas
{
    internal class SimpleRoom(Room room)
    {
        [JsonIgnore]
        internal readonly Room room = room;

        public string Name { get; } = room.Identifier.name;

        public SimpleVector Position { get; } = new(room.Position);

        public SimpleVector Rotation { get; } = SimpleVector.FromQuaternion(room.Rotation);

        public RoomShape Shape { get; } = room.RoomShape;

        public bool LightsOn { get; } = !room.AreLightsOff;
    }
}
