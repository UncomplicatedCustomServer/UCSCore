using Exiled.API.Features;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using UncomplicatedCustomServerCore.API.Features.Round.Messages;
using UncomplicatedCustomServerCore.Extensions;
using UncomplicatedCustomServerCore.Schemas;

namespace UncomplicatedCustomServerCore.API.Utilities
{
    internal class ChangeDetector
    {
        public static List<SerializedPlayer> RefPlayers { get; } = [];

        public static List<SimpleDoor> RefDoors { get; } = [];

        public static List<SimpleRoom> RefRooms { get; } = [];

        public static List<SimpleLift> RefLifts { get; } = [];

        public static List<SimplePickup> RefPickups { get; } = [];

        public static bool DoEnableTimer { get; internal set; } = true;

        public static readonly int UpdateInterval = 2500;

        public static bool Compare(object object1, object object2)
        {
            //return object1.Equals(object2);
            if (object1.GetType() != object2.GetType())
                return false;

            try
            {
                foreach (PropertyInfo property in object1.GetType().GetProperties().Where(p => !p.Name.ToLower().Contains("rotation")))
                    if (property.GetValue(object1, null)?.ToString() != property.GetValue(object2, null)?.ToString())
                    {
                        if (object1 is SimplePlayer)
                            Log.Warn($"DIFFERENCE IN {property.Name} (({property.GetValue(object1, null).GetType().Name} {property.GetValue(object1, null)} and ({property.GetValue(object2, null).GetType().Name}) {property.GetValue(object2, null)})");
                        return false;
                    }
            }
            catch (System.Exception e)
            {
                Log.Error($"Failed to compare objects: {e}");
                return false;
            }

            return true;
        }

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
                try
                {
                    Log.Info("Change message task started!");
                    while (DoEnableTimer)
                    {
                        MapUpdateMessage partialMessage = MapUpdateMessage.PushAll();

                        foreach (SimpleDoor door in partialMessage.Doors.ToList())
                            if (!new SimpleDoor(door.door).HasChanged())
                                partialMessage.Doors.RemoveAll(d => d.Identifier == door.Identifier);

                        foreach (SimpleRoom room in partialMessage.Rooms.ToList())
                            if (!new SimpleRoom(room.room).HasChanged())
                                partialMessage.Rooms.Remove(room);

                        foreach (SimpleLift lift in partialMessage.Lifts.ToList())
                            if (!new SimpleLift(lift.lift).HasChanged())
                                partialMessage.Lifts.RemoveAll(l => l.Name == lift.Name && l.Group == lift.Group.ToString());

                        foreach (SimplePickup pickup in partialMessage.Pickups.ToList())
                            if (!new SimplePickup(pickup.pickup).HasChanged())
                                partialMessage.Pickups.RemoveAll(p => p.Serial == pickup.Serial);

                        foreach (SerializedPlayer player in RefPlayers.ToList())
                            if (new SerializedPlayer(player.player).HasChanged())
                                PlayerUpdateMessage.Create(player.player).Send();

                        if (partialMessage.Doors.Count > 0 || partialMessage.Rooms.Count > 0 || partialMessage.Lifts.Count > 0)
                            partialMessage.Send();

                        await Task.Delay(UpdateInterval);
                    }
                }
                catch (System.Exception e)
                {
                    Log.Error($"Failed to run change loop change detector task: {e}");
                }
            });
        }
    }
}
