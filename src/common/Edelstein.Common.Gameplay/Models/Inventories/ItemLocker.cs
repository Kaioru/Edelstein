using Edelstein.Protocol.Gameplay.Models.Inventories;

namespace Edelstein.Common.Gameplay.Models.Inventories;

public record ItemLocker : IItemLocker
{
    public short SlotMax { get; set; } = 999;
    public ICollection<IItemLockerSlot> Items { get; } = new List<IItemLockerSlot>();
}
