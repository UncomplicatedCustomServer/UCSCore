using Exiled.API.Features;
using System.Linq;

namespace UncomplicatedCustomServerCore.Schemas
{
#nullable enable

    internal class SerializedPlayer(Player player) : CompletePlayer(player)
    {
        public new string Team { get; } = player.Role.Team.ToString();

        public new string Role { get; } = player.Role.Type.ToString();

        public new string Zone { get; } = player.Zone.ToString();

        public new string? Room { get; } = player.CurrentRoom?.Type.ToString();

        public new string[] Inventory { get; } = player.Inventory?.UserInventory.Items.Values.Select(i => i.ItemTypeId.ToString()).ToArray() ?? [];

        public new string? CurrentItem { get; } = player.CurrentItem?.Type.ToString();
    }
}
