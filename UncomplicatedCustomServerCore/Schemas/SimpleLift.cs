using Exiled.API.Features;
using Interactables.Interobjects;
using Newtonsoft.Json;
using static Interactables.Interobjects.ElevatorChamber;

namespace UncomplicatedCustomServerCore.Schemas
{
    internal class SimpleLift(Lift lift)
    {
        [JsonIgnore]
        internal readonly Lift lift = lift;

        public string Name { get; } = lift.Name;

        public ElevatorGroup Group { get; } = lift.Group;

        public SimpleVector Position { get; } = new(lift.Position);

        public SimpleVector Rotation { get; } = SimpleVector.FromQuaternion(lift.Rotation);

        public bool IsMoving { get; } = lift.IsMoving;

        public bool IsLocked { get; } = lift.IsLocked;

        public int CurrentLevel { get; } = lift.CurrentLevel;

        public bool CanStart { get; } = lift.Status is ElevatorSequence.Ready && lift.IsOperative;
    }
}
