using System.Linq;
using UncomplicatedCustomServerCore.API.Utilities;
using UncomplicatedCustomServerCore.Schemas;

namespace UncomplicatedCustomServerCore.Extensions
{
    internal static class SimpleModelsExtension
    {
        public static bool HasChanged(this SimpleDoor door)
        {
            SimpleDoor oldDoor = ChangeDetector.RefDoors.FirstOrDefault(d => d.Identifier == door.Identifier);

            ChangeDetector.RefDoors.Add(door);

            if (oldDoor is null)
                return false;

            ChangeDetector.RefDoors.Remove(oldDoor);

            return !ChangeDetector.Compare(oldDoor, door);
        }

        public static bool HasChanged(this SimpleRoom room)
        {
            SimpleRoom oldRoom = ChangeDetector.RefRooms.FirstOrDefault(r => r.Name == room.Name);

            ChangeDetector.RefRooms.Add(room);

            if (oldRoom is null)
                return false;

            ChangeDetector.RefRooms.Remove(oldRoom);

            return !ChangeDetector.Compare(oldRoom, room);
        }

        public static bool HasChanged(this SimpleLift lift)
        {
            SimpleLift oldLift = ChangeDetector.RefLifts.FirstOrDefault(l => l.Name == lift.Name);

            ChangeDetector.RefLifts.Add(lift);

            if (oldLift is null)
                return false;

            ChangeDetector.RefLifts.Remove(oldLift);

            return !ChangeDetector.Compare(oldLift, lift);
        }

        public static bool HasChanged(this CompletePlayer player)
        {
            CompletePlayer oldPlayer = ChangeDetector.RefPlayers.FirstOrDefault(p => p.Id == player.Id);

            ChangeDetector.RefPlayers.Add(player);

            if (oldPlayer is null)
                return false;

            ChangeDetector.RefPlayers.Remove(oldPlayer);

            return !ChangeDetector.Compare(oldPlayer, player);
        }

        public static bool HasChanged(this SimplePickup pickup)
        {
            SimplePickup oldPickup = ChangeDetector.RefPickups.FirstOrDefault(p => p.Serial == pickup.Serial);

            ChangeDetector.RefPickups.Add(pickup);

            if (oldPickup is null)
                return false;

            ChangeDetector.RefPickups.Remove(oldPickup);

            return !ChangeDetector.Compare(oldPickup, pickup);
        }
    }
}
