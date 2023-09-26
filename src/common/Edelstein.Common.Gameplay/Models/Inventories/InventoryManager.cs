using System.Collections.Immutable;
using Edelstein.Common.Gameplay.Models.Inventories.Items;
using Edelstein.Protocol.Gameplay.Models.Characters;
using Edelstein.Protocol.Gameplay.Models.Inventories;
using Edelstein.Protocol.Gameplay.Models.Inventories.Items;
using Edelstein.Protocol.Gameplay.Models.Inventories.Templates;
using Edelstein.Protocol.Utilities.Templates;

namespace Edelstein.Common.Gameplay.Models.Inventories;

public class InventoryManager : IInventoryManager
{
    private readonly ITemplateManager<IItemTemplate> _templates;

    public InventoryManager(ITemplateManager<IItemTemplate> templates) => _templates = templates;
    
    private ItemInventoryType GetTypeByID(int id)
        => (ItemInventoryType)(id / 1_000_000);

    public bool HasItem(ICharacterInventories inventory, int templateID)
        => HasItem(inventory[GetTypeByID(templateID)], templateID);
    
    public bool HasItem(ICharacterInventories inventory, int templateID, short count) 
        => HasItem(inventory[GetTypeByID(templateID)], templateID, count);
    
    public bool HasItem(ICharacterInventories inventory, IItemTemplate template)
        => HasItem(inventory[GetTypeByID(template.ID)], template.ID);
    
    public bool HasItem(ICharacterInventories inventory, IItemTemplate template, short count)
        => HasItem(inventory[GetTypeByID(template.ID)], template.ID, count);

    public bool HasSlotFor(ICharacterInventories inventory, int templateID)
        => HasSlotFor(inventory[GetTypeByID(templateID)], templateID);
    
    public bool HasSlotFor(ICharacterInventories inventory, int templateID, short count)
        => HasSlotFor(inventory[GetTypeByID(templateID)], templateID, count);
    
    public bool HasSlotFor(ICharacterInventories inventory, ICollection<Tuple<int, short>> templates) 
        => templates
            .GroupBy(t => GetTypeByID(t.Item1))
            .All(g => HasSlotFor(inventory[g.Key], g.ToImmutableList()));
    
    public bool HasSlotFor(ICharacterInventories inventory, IItemTemplate template) 
        => HasSlotFor(inventory[GetTypeByID(template.ID)], template.ID);
    
    public bool HasSlotFor(ICharacterInventories inventory, IItemTemplate template, short count)
        => HasSlotFor(inventory[GetTypeByID(template.ID)], template.ID, count);
    
    public bool HasSlotFor(ICharacterInventories inventory, ICollection<Tuple<IItemTemplate, short>> templates)
        => templates
            .GroupBy(t => GetTypeByID(t.Item1.ID))
            .All(g => HasSlotFor(inventory[g.Key], g.ToImmutableList()));
    
    public bool HasSlotFor(ICharacterInventories inventory, IItemSlot item)
        => HasSlotFor(inventory[GetTypeByID(item.ID)], item);
    
    public bool HasSlotFor(ICharacterInventories inventory, ICollection<IItemSlot> items)
        => items
            .GroupBy(t => GetTypeByID(t.ID))
            .All(g => HasSlotFor(inventory[g.Key], g.ToImmutableList()));

    public bool HasItem(IItemInventory? inventory, int templateID)
        => HasItem(inventory, templateID, 1);
    
    public bool HasItem(IItemInventory? inventory, int templateID, short count) 
        => inventory?.Items
            .Where(kv => kv.Key > 0)
            .Count(i => i.Value.ID == templateID) >= count;

    public bool HasItem(IItemInventory? inventory, IItemTemplate template)
        => HasItem(inventory, template.ID);
    
    public bool HasItem(IItemInventory? inventory, IItemTemplate template, short count)
        => HasItem(inventory, template.ID, count);
    
    public bool HasSlotFor(IItemInventory? inventory, int templateID) 
        => HasItem(inventory, templateID);
    
    public bool HasSlotFor(IItemInventory? inventory, int templateID, short count)
        => HasItem(inventory, templateID, count);
    
    public bool HasSlotFor(IItemInventory? inventory, ICollection<Tuple<int, short>> templates) 
        => HasSlotFor(inventory, templates
                .Select(t => Tuple.Create(_templates.Retrieve(t.Item1).Result!, t.Item2))
                .ToImmutableList());
    
    public bool HasSlotFor(IItemInventory? inventory, IItemTemplate template)
        => HasSlotFor(inventory, template, 1);
    
    public bool HasSlotFor(IItemInventory? inventory, IItemTemplate template, short count)
        => HasSlotFor(inventory, ImmutableList.Create(Tuple.Create(template, count)));
    
    public bool HasSlotFor(IItemInventory? inventory, ICollection<Tuple<IItemTemplate, short>> templates) 
        => HasSlotFor(inventory, templates
                .Select(t =>
                {
                    var items = new List<IItemSlot>();

                    if (t.Item1 is IItemBundleTemplate bundle)
                    {
                        var total = t.Item2;

                        while (total > 0)
                        {
                            var count = Math.Min(total, bundle.MaxPerSlot);
                        
                            total -= count;
                            items.Add(bundle.ToItemSlotBundle(count));
                        }
                    }
                    else
                        for (var i = 0; i < t.Item2; i++)
                            items.Add(t.Item1.ToItemSlot());
                
                    return items;
                })
                .SelectMany(i => i)
                .ToImmutableList());
    
    public bool HasSlotFor(IItemInventory? inventory, IItemSlot item) 
        => HasSlotFor(inventory, ImmutableList.Create(item));
    
    public bool HasSlotFor(IItemInventory? inventory, ICollection<IItemSlot> items)
    {
        if (inventory == null) return false;
        
        var bundles = items
            .OfType<IItemSlotBundle>()
            .ToImmutableList();
        var bundlesMerged = new List<IItemSlotBundle>();
        
        foreach (var bundle in bundles)
        {
            var mergeable = bundlesMerged
                .FirstOrDefault(i => i.MergeableWith(bundle));

            if (mergeable == null)
            {
                bundlesMerged.Add(new ItemSlotBundle
                {
                    ID = bundle.ID,
                    DateExpire = bundle.DateExpire,
                    Number = bundle.Number,
                    Attribute = bundle.Attribute,
                    Title = bundle.Title
                });
                continue;
            }

            mergeable.Number += bundle.Number;
        }

        var cache = new Dictionary<IItemSlotBundle, int>();
        var totalSlots = items
            .Except(bundles)
            .Count();
        
        foreach (var bundle in bundlesMerged)
        {
            var count = (int)bundle.Number;
            var template = (IItemBundleTemplate)_templates.Retrieve(bundle.ID).Result!;
            
            count = inventory.Items.Values
                .OfType<IItemSlotBundle>()
                .Where(i => i.MergeableWith(bundle))
                .Where(i => (cache.TryGetValue(i, out var number) ? number : i.Number) < template.MaxPerSlot)
                .Aggregate(count, (current, merge) =>
                {
                    cache.Add(merge, template.MaxPerSlot);
                    return current - Math.Min(current, template.MaxPerSlot - merge.Number);
                });
            totalSlots += (int)Math.Ceiling(count / (double)template.MaxPerSlot);
        }
        
        return inventory.Items.Count(kv => kv.Key > 0) + totalSlots <= inventory.SlotMax;
    }
}
