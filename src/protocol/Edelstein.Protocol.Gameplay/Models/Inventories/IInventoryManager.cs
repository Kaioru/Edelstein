using Edelstein.Protocol.Gameplay.Models.Characters;
using Edelstein.Protocol.Gameplay.Models.Inventories.Items;
using Edelstein.Protocol.Gameplay.Models.Inventories.Templates;

namespace Edelstein.Protocol.Gameplay.Models.Inventories;

public interface IInventoryManager
{
    bool HasItem(ICharacterInventories inventory, int templateID);
    bool HasItem(ICharacterInventories inventory, int templateID, short count);
    bool HasItem(ICharacterInventories inventory, IItemTemplate template);
    bool HasItem(ICharacterInventories inventory, IItemTemplate template, short count);

    bool HasSlotFor(ICharacterInventories inventory, int templateID);
    bool HasSlotFor(ICharacterInventories inventory, int templateID, short count);
    
    bool HasSlotFor(ICharacterInventories inventory, ICollection<Tuple<int, short>> templates);
    
    bool HasSlotFor(ICharacterInventories inventory, IItemTemplate template);
    bool HasSlotFor(ICharacterInventories inventory, IItemTemplate template, short count);
    bool HasSlotFor(ICharacterInventories inventory, ICollection<Tuple<IItemTemplate, short>> templates);

    bool HasSlotFor(ICharacterInventories inventory, IItemSlot item);
    bool HasSlotFor(ICharacterInventories inventory, ICollection<IItemSlot> items);
    
    bool HasItem(IItemInventory inventory, int templateID);
    bool HasItem(IItemInventory inventory, int templateID, short count);
    bool HasItem(IItemInventory inventory, IItemTemplate template);
    bool HasItem(IItemInventory inventory, IItemTemplate template, short count);

    bool HasSlotFor(IItemInventory inventory, int templateID);
    bool HasSlotFor(IItemInventory inventory, int templateID, short count);
    
    bool HasSlotFor(IItemInventory inventory, ICollection<Tuple<int, short>> templates);
    
    bool HasSlotFor(IItemInventory inventory, IItemTemplate template);
    bool HasSlotFor(IItemInventory inventory, IItemTemplate template, short count);
    bool HasSlotFor(IItemInventory inventory, ICollection<Tuple<IItemTemplate, short>> templates);

    bool HasSlotFor(IItemInventory inventory, IItemSlot item);
    bool HasSlotFor(IItemInventory inventory, ICollection<IItemSlot> items);
}
