namespace Edelstein.Protocol.Gameplay.Inventories.Items;

public interface IItemSlotBundle : IItemSlotBase
{
    short Number { get; set; }
    short Attribute { get; set; }

    string Title { get; set; }

    bool MergeableWith(IItemSlotBundle bundle);
}
