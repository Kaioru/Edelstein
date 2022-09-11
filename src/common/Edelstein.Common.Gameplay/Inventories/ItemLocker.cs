using Edelstein.Protocol.Gameplay.Inventories;

namespace Edelstein.Common.Gameplay.Inventories;

public record ItemLocker : IItemLocker
{
    public ItemLocker()
    {
    }

    public ItemLocker(short slotMax)
    {
        SlotMax = slotMax;
        Items = new Dictionary<short, IItemLockerSlot>();
    }

    public short SlotMax { get; set; }
    public IDictionary<short, IItemLockerSlot> Items { get; }
}
