using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Edelstein.Protocol.Gameplay.Constants;
using Edelstein.Protocol.Gameplay.Entities;
using Edelstein.Protocol.Gameplay.Entities.Inventories;
using Edelstein.Protocol.Gameplay.Entities.Inventories.Accessors;
using Edelstein.Protocol.Gameplay.Entities.Inventories.Templates;
using Edelstein.Protocol.Utilities.Templates;

namespace Edelstein.Common.Gameplay.Entities.Inventories.Accessors;

public class AccessInventory(
    CharacterInventories inventories,
    ITemplateManager<IItemTemplate> templates
) : IAccessInventory
{
    public Task<int> Count(int itemID) 
        => Task.FromResult(inventories[itemID.GetInventoryType()]?.Items
            .Where(kv => kv.Key > 0)
            .Where(i => i.Value.TemplateID == itemID)
            .Sum(i => i.Value is ItemSlotBundle bundle ? bundle.Number : 1) ?? 0);
    
    public Task<int> SlotsUsed(ItemInventoryType type)
        => Task.FromResult(inventories[type]?.Items
            .Count(kv => kv.Key > 0) ?? 0);

    public async Task<int> SlotsRemaining(ItemInventoryType type)
        => (inventories[type]?.SlotMax ?? 0) - await SlotsUsed(type);

    public async Task<bool> Has(int itemID, short number = 1)
        => await Count(itemID) >= number;

    public Task<bool> HasEquipped(int itemID)
        => Task.FromResult(inventories[ItemInventoryType.Equip]?.Items
            .Where(kv => kv.Key < 0)
            .Any(kv => kv.Value.TemplateID == itemID) ?? false);

    public Task<bool> CanHold(int itemID, short number = 1)
        => CanHold(ImmutableHashSet.Create(Tuple.Create(itemID, number)));
    
    public async Task<bool> CanHold(IEnumerable<Tuple<int, short>> items)
    {
        var required = new Dictionary<ItemInventoryType, int>
        {
            [ItemInventoryType.Equip] = 0,
            [ItemInventoryType.Consume] = 0,
            [ItemInventoryType.Install] = 0,
            [ItemInventoryType.Etc] = 0,
            [ItemInventoryType.Cash] = 0
        };

        foreach (var (itemID, number) in items)
        {
            var template = await templates.Retrieve(itemID);
            
            required[itemID.GetInventoryType()] += template is IItemBundleTemplate bundle
                ? number / bundle.MaxPerSlot
                : 1;
        }

        return required
            .All(kv => inventories[kv.Key]?.Items
                .Count(i => i.Key > 0) >= kv.Value);
    }
}
