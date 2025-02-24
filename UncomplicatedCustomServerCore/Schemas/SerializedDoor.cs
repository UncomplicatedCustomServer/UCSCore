using Exiled.API.Features.Doors;

namespace UncomplicatedCustomServerCore.Schemas
{
    internal class SerializedDoor(Door door): SimpleDoor(door)
    {
        public new string Type { get; } = door.Type.ToString();
    }
}
