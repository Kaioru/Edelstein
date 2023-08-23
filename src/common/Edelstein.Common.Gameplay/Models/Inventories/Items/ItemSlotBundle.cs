using Edelstein.Protocol.Gameplay.Models.Inventories.Items;

namespace Edelstein.Common.Gameplay.Models.Inventories.Items;

public record ItemSlotBundle : ItemSlotBase, IItemSlotBundle
{
    public short Number { get; set; }
    public short Attribute { get; set; }

    public string Title { get; set; }

    public bool MergeableWith(IItemSlotBundle bundle) =>
        Number == bundle.Number &&
        Attribute == bundle.Attribute &&
        Title == bundle.Title &&
        DateExpire == bundle.DateExpire;
}
