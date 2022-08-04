using Edelstein.Protocol.Gameplay.Inventories.Items;

namespace Edelstein.Common.Gameplay.Inventories.Items;

public record ItemSlotBundle : ItemSlotBase, IItemSlotBundle
{
    public short Number { get; set; }
    public short Attribute { get; set; }

    public string Title { get; set; }

    public bool MergeableWith(IItemSlotBundle bundle) => throw new NotImplementedException();
}
