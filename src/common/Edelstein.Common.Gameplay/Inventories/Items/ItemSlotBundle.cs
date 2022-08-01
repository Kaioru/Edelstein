using Edelstein.Protocol.Gameplay.Inventories.Items;

namespace Edelstein.Common.Gameplay.Inventories.Items;

public record ItemSlotBundle : IItemSlotBundle
{
    public int ID { get; set; }

    public short Number { get; set; }
    public short Attribute { get; set; }

    public string Title { get; set; }
}
