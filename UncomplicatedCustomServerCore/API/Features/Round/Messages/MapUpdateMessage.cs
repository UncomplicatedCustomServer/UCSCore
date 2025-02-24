using Exiled.API.Features;
using Exiled.API.Features.Doors;
using MapGeneration;
using System.Collections.Generic;
using System.Linq;
using UncomplicatedCustomServerCore.Schemas;

namespace UncomplicatedCustomServerCore.API.Features.Round.Messages
{
    internal class MapUpdateMessage : MessageBase
    {
        public override RoundActionType Action => RoundActionType.MapUpdate;

        public List<SimpleRoom> Rooms { get; } = [];

        public List<SerializedDoor> Doors { get; } = [];

        public List<SerializedLift> Lifts { get; } = [];

        public static MapUpdateMessage PushAll()
        {
            MapUpdateMessage message = new();

            foreach (Room room in Room.List)
                message.Rooms.Add(new(room));

            foreach (Door door in Door.List.Where(d => !d.IsElevator))
                message.Doors.Add(new(door));

            foreach (Lift lift in Lift.List)
                message.Lifts.Add(new(lift));

            return message;
        }

        public static MapUpdateMessage PushSpecific(IEnumerable<Room> rooms, IEnumerable<Door> doors, IEnumerable<Lift> lifts)
        {
            MapUpdateMessage message = new();

            foreach (Room room in rooms)
                message.Rooms.Add(new(room));

            foreach (Door door in doors.Where(d => !d.IsElevator))
                message.Doors.Add(new(door));

            foreach (Lift lift in lifts)
                message.Lifts.Add(new(lift));

            return message;
        }

        public static MapUpdateMessage PushSingle(Room room)
        {
            MapUpdateMessage message = new();

            message.Rooms.Add(new(room));

            return message;
        }

        public static MapUpdateMessage PushSingle(Door door)
        {
            MapUpdateMessage message = new();

            message.Doors.Add(new(door));

            return message;
        }

        public static MapUpdateMessage PushSingle(Lift lift)
        {
            MapUpdateMessage message = new();

            message.Lifts.Add(new(lift));

            return message;
        }
    }
}
