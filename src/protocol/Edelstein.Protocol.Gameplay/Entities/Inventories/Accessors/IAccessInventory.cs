using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Edelstein.Protocol.Gameplay.Entities.Inventories.Accessors;

public interface IAccessInventory
{
    Task<int> Count(int itemID);
    
    Task<int> SlotsUsed(ItemInventoryType type);
    Task<int> SlotsRemaining(ItemInventoryType type);
    
    Task<bool> Has(int itemID, short number = 1);
    Task<bool> HasEquipped(int itemID);

    Task<bool> CanHold(int itemID, short number = 1);
    Task<bool> CanHold(IEnumerable<Tuple<int, short>> items);
    
    Task<bool> CanHold(ItemSlotBase item);
    Task<bool> CanHold(IEnumerable<ItemSlotBase> items);
}
