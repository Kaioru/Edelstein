using Edelstein.Protocol.Gameplay.Models.Inventories.Items;

namespace Edelstein.Common.Gameplay.Models.Inventories.Items;

public record ItemSlot : IItemSlot
{
    public int ID { get; set; }
}
