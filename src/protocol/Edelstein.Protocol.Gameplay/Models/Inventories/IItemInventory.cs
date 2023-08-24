using Edelstein.Protocol.Gameplay.Models.Inventories.Items;

namespace Edelstein.Protocol.Gameplay.Models.Inventories;

public interface IItemInventory : IItemInventory<IItemSlot>
{
}

public interface IItemInventory<TSlot> where TSlot : IItemSlot
{
    short SlotMax { get; set; }

    IDictionary<short, TSlot> Items { get; }
}
