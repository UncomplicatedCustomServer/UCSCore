using Exiled.API.Features;
using System.Collections.Generic;
using System.Threading.Tasks;
using UncomplicatedCustomServerCore.API.Features.Round.Messages;
using UncomplicatedCustomServerCore.Extensions;
using UncomplicatedCustomServerCore.Schemas;

namespace UncomplicatedCustomServerCore.API.Utilities
{
    internal class ChangeDetector
    {
        public static List<CompletePlayer> RefPlayers { get; } = [];

        public static List<SimpleDoor> RefDoors { get; } = [];

        public static List<SimpleRoom> RefRooms { get; } = [];

        public static List<SimpleLift> RefLifts { get; } = [];

        public static List<SimplePickup> RefPickups { get; } = [];

        public static bool DoEnableTimer { get; internal set; } = true;

        public static readonly int UpdateInterval = 3500;

        public static bool Compare(object object1, object object2) => object1.Equals(object2);

        public static void Intialize()
        {
            MapUpdateMessage partialMessage = MapUpdateMessage.PushAll();

            foreach (SimpleDoor door in partialMessage.Doors)
                RefDoors.Add(door);

            foreach (SimpleRoom room in partialMessage.Rooms)
                RefRooms.Add(room);

            foreach (SimpleLift lift in partialMessage.Lifts)
                RefLifts.Add(lift);

            foreach (SimplePickup pickup in partialMessage.Pickups)
                RefPickups.Add(pickup);

            Task.Run(async delegate
            {
                Log.Info("Round update message task started!");
                while (DoEnableTimer)
                {
                    RoundUpdateMessage.Create().Send();
                    await Task.Delay(2000);
                }
            });

            Task.Run(async delegate
            {
                Log.Info("Change message task started!");
                while (DoEnableTimer)
                {
                    MapUpdateMessage partialMessage = MapUpdateMessage.PushAll();

                    foreach (SimpleDoor door in partialMessage.Doors)
                        if (!door.HasChanged())
                            partialMessage.Doors.RemoveAll(d => d.Identifier == door.Identifier);

                    foreach (SimpleRoom room in partialMessage.Rooms)
                        if (!room.HasChanged())
                            partialMessage.Rooms.Remove(room);

                    foreach (SimpleLift lift in partialMessage.Lifts)
                        if (!lift.HasChanged())
                            partialMessage.Lifts.RemoveAll(l => l.Name == lift.Name && l.Group == lift.Group.ToString());

                    foreach (SimplePickup pickup in partialMessage.Pickups)
                        if (!pickup.HasChanged())
                            partialMessage.Pickups.RemoveAll(p => p.Serial == pickup.Serial);

                    foreach (CompletePlayer player in RefPlayers)
                        if (player.HasChanged())
                            new PlayerUpdateMessage(player).Send();

                    if (partialMessage.Doors.Count > 0 || partialMessage.Rooms.Count > 0 || partialMessage.Lifts.Count > 0)
                        partialMessage.Send();

                    await Task.Delay(UpdateInterval);
                }
            });
        }
    }
}
