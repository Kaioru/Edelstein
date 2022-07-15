using Edelstein.Protocol.Gameplay.Inventories.Items;

namespace Edelstein.Protocol.Gameplay.Inventories;

public interface IItemInventory : IItemInventory<IItemSlot>
{
}

public interface IItemInventory<TSlot> where TSlot : IItemSlot
{
    short SlotMax { get; }

    IDictionary<short, IItemSlot> Items { get; }
}
