using Edelstein.Protocol.Gameplay.Inventories;

namespace Edelstein.Common.Gameplay.Inventories;

public record ItemLocker : IItemLocker
{
    public short SlotMax { get; set; }
    public IDictionary<short, IItemLockerSlot> Items { get; }
}
