namespace Edelstein.Protocol.Gameplay.Inventories.Items;

public interface IItemSlotBundle : IItemSlot
{
    short Number { get; set; }
    short Attribute { get; set; }

    string Title { get; set; }
}
