using Exiled.API.Features.Pickups;

namespace UncomplicatedCustomServerCore.Schemas
{
    internal class SerializedPickup(Pickup pickup) : SimplePickup(pickup)
    {
        public new string Type { get; } = pickup.Type.ToString();
    }
}
