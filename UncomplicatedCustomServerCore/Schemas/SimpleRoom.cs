using Exiled.API.Enums;
using Exiled.API.Features;
using MapGeneration;
using Newtonsoft.Json;
using UncomplicatedCustomServerCore.Extensions;

namespace UncomplicatedCustomServerCore.Schemas
{
    internal class SimpleRoom(Room room)
    {
        [JsonIgnore]
        internal readonly Room room = room;

        public string Identifier { get; } = new SimpleVector(room.Position).ToString().Base64Encode();

        public SimpleVector Position { get; } = new(room.Position);

        public SimpleVector Rotation { get; } = SimpleVector.FromQuaternion(room.Rotation);

        public RoomShape Shape { get; } = room.RoomShape;

        public RoomName MapGenRoomName { get; } = room.RoomName;

        public string MapGenRoomTypePlain { get; } = room.Type.ToString();

        public ZoneType Zone { get; } = room.Zone;

        public SimpleColor Color { get; } = new(room.Color);

        public bool LightsOn { get; } = !room.AreLightsOff;
    }
}
