using Exiled.API.Features;

namespace UncomplicatedCustomServerCore.Schemas
{
    internal class SerializedLift(Lift lift) : SimpleLift(lift)
    {
        public new string Group { get; } = lift.Group.ToString();
    }
}
