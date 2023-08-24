using System.Collections.Immutable;
using Edelstein.Common.Gameplay.Models.Inventories.Items;
using Edelstein.Common.Gameplay.Models.Inventories.Modify.Operations;
using Edelstein.Protocol.Gameplay.Models.Inventories;
using Edelstein.Protocol.Gameplay.Models.Inventories.Items;
using Edelstein.Protocol.Gameplay.Models.Inventories.Modify;
using Edelstein.Protocol.Gameplay.Models.Inventories.Templates;
using Edelstein.Protocol.Utilities.Templates;

namespace Edelstein.Common.Gameplay.Models.Inventories.Modify;

public class ModifyInventoryContext : AbstractModifyInventory, IModifyInventoryContext
{
    private readonly IItemInventory _inventory;

    private readonly ITemplateManager<IItemTemplate> _manager;
    private readonly Queue<AbstractModifyInventoryOperation> _operations;
    private readonly ItemInventoryType _type;

    public ModifyInventoryContext(
        ItemInventoryType type,
        IItemInventory inventory,
        ITemplateManager<IItemTemplate> manager
    )
    {
        _type = type;
        _inventory = inventory;
        _manager = manager;
        _operations = new Queue<AbstractModifyInventoryOperation>();
    }

    public override IEnumerable<AbstractModifyInventoryOperation> Operations => _operations.AsEnumerable();

    public IItemSlot? this[short slot] => _inventory.Items.TryGetValue(slot, out var item)
        ? item
        : null;

    public IReadOnlyDictionary<short, IItemSlot> Items => _inventory.Items.ToImmutableDictionary();

    public override bool HasItem(int templateID) =>
        HasItem(templateID, 1);

    public override bool HasItem(int templateID, short count) =>
        _inventory.Items.Count(i => i.Value.ID == templateID) >= count;

    public override bool HasItem(IItemTemplate template) =>
        HasItem(template.ID);

    public override bool HasItem(IItemTemplate template, short count) =>
        HasItem(template.ID, count);

    public override bool HasSlotFor(int templateID) =>
        HasSlotFor(templateID, 1);

    public override bool HasSlotFor(int templateID, short count) =>
        HasSlotFor(ImmutableList.Create(Tuple.Create(templateID, count)));
    
    public override bool HasSlotFor(ICollection<Tuple<int, short>> templates) =>
        HasSlotFor(templates
            .Select(t => Tuple.Create(_manager.Retrieve(t.Item1).Result!, t.Item2))
            .ToImmutableList());
    
    public override bool HasSlotFor(IItemTemplate template) =>
        HasItem(template, 1);
    
    public override bool HasSlotFor(IItemTemplate template, short count) => 
        HasSlotFor(ImmutableList.Create(Tuple.Create(template, count)));
    
    public override bool HasSlotFor(ICollection<Tuple<IItemTemplate, short>> templates) =>
        HasSlotFor(templates
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
    
    public override bool HasSlotFor(IItemSlot item) =>
        HasSlotFor(ImmutableList.Create(item));

    public override bool HasSlotFor(ICollection<IItemSlot> items)
    {
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
            var template = (IItemBundleTemplate)_manager.Retrieve(bundle.ID).Result!;
            
            count = _inventory.Items.Values
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
        
        return _inventory.Items.Count + totalSlots <= _inventory.SlotMax;
    }

    public override void Add(IItemSlot? item)
    {
        switch (item)
        {
            case ItemSlotBundle bundle:
                if (_manager.Retrieve(bundle.ID).Result is not IItemBundleTemplate template) goto default;
                // if (ItemConstants.IsRechargeableItem(bundle.TemplateID)) goto default; // TODO: rechargeable constants
                if (bundle.Number < 1) bundle.Number = 1;

                var mergeable = _inventory.Items
                    .Where(kv => kv.Value is IItemSlotBundle b && b.Number < template.MaxPerSlot)
                    .Select(kv => Tuple.Create(kv.Key, (IItemSlotBundle)kv.Value))
                    .FirstOrDefault(t => t.Item2.MergeableWith(bundle));

                if (mergeable != null)
                {
                    var count = bundle.Number + mergeable.Item2.Number;
                    var maxNumber = template.MaxPerSlot;

                    if (count > maxNumber)
                    {
                        var leftover = count - maxNumber;

                        bundle.Number = (short)leftover;
                        mergeable.Item2.Number = maxNumber;
                        UpdateQuantitySlot(mergeable.Item1, mergeable.Item2.Number);
                        Add(bundle);
                        return;
                    }

                    mergeable.Item2.Number += bundle.Number;
                    UpdateQuantitySlot(mergeable.Item1, mergeable.Item2.Number);
                    return;
                }

                goto default;
            default:
                var slot = Enumerable.Range(1, _inventory.SlotMax)
                    .Select(i => (short)i)
                    .Except(_inventory.Items.Keys)
                    .FirstOrDefault(i => i > 0);

                if (slot > 0 && item != null) SetSlot(slot, item);
                break;
        }
    }

    public override void Remove(int templateID) =>
        Remove(templateID, 1);

    public override void Remove(int templateID, short count)
    {
        var removed = 0;
        var match = _inventory.Items
            .Where(kv => kv.Value.ID == templateID)
            .ToImmutableList();

        foreach (var kv in match)
        {
            if (removed >= count) return;
            if (kv.Value is ItemSlotBundle bundle) // TODO: rechargable
            {
                var diff = count - removed;

                if (bundle.Number > diff)
                {
                    removed += diff;
                    bundle.Number = (short)(bundle.Number - diff);
                    UpdateQuantitySlot(kv.Key, bundle.Number);
                }
                else
                {
                    removed += bundle.Number;
                    RemoveSlot(kv.Key);
                }
            }
            else
            {
                removed++;
                RemoveSlot(kv.Key);
            }
        }
    }

    public override void Remove(IItemTemplate template) =>
        Remove(template.ID, 1);

    public override void Remove(IItemTemplate template, short count) =>
        Remove(template.ID, count);

    public override void RemoveAll(int templateID)
    {
        var match = _inventory.Items
            .Where(kv => kv.Value.ID == templateID)
            .ToImmutableList();

        foreach (var kv in match)
            RemoveSlot(kv.Key);
    }

    public override void RemoveAll(IItemTemplate template) =>
        RemoveAll(template.ID);

    public override void Gather()
    {
        var inventoryCopy = _inventory.Items
            .Where(kv => kv.Key > 0)
            .OrderBy(kv => kv.Key)
            .ToList();
        short position = 1;

        inventoryCopy.ForEach(kv => RemoveSlot(kv.Key));
        inventoryCopy.ForEach(kv => SetSlot(position++, kv.Value));
    }

    public override void Sort()
    {
        var inventoryCopy = _inventory.Items
            .Where(kv => kv.Key > 0)
            .OrderBy(kv => kv.Value.ID)
            .ToList();

        inventoryCopy.ForEach(kv => RemoveSlot(kv.Key));
        inventoryCopy.ForEach(kv => Add(kv.Value));
    }
    public override void Clear() =>
        _inventory.Items
            .Where(kv => kv.Key > 0)
            .ToList()
            .ForEach(kv => RemoveSlot(kv.Key));

    public override void Add(int templateID) =>
        Add(templateID, 1);

    public override void Add(int templateID, short count) =>
        Add(_manager.Retrieve(templateID).Result, count);

    public override void Add(IItemTemplate? template) =>
        Add(template, 1);

    public override void Add(IItemTemplate? template, short count)
    {
        var item = template?.ToItemSlot();

        if (item is IItemSlotBundle bundle)
            bundle.Number = count;
        if (item != null) Add(item);
    }

    public void SetSlot(short slot, IItemSlot item)
    {
        _inventory.Items[slot] = item;
        _operations.Enqueue(new AddModifyInventoryOperation(_type, slot, item));
    }

    public void RemoveSlot(short slot)
    {
        _inventory.Items.Remove(slot);
        _operations.Enqueue(new RemoveModifyInventoryOperation(_type, slot));
    }

    public void MoveSlot(short from, short to)
    {
        var itemFrom = this[from];
        var itemTo = this[to];

        _inventory.Items.Remove(from);
        _inventory.Items.Remove(to);

        if (itemTo != null) _inventory.Items[from] = itemTo;
        if (itemFrom != null) _inventory.Items[to] = itemFrom;

        _operations.Enqueue(new MoveModifyInventoryOperation(_type, from, to));
    }

    public void UpdateSlot(short slot)
    {
        var item = _inventory.Items[slot];

        RemoveSlot(slot);
        SetSlot(slot, item);
    }

    public void SetSlot(short slot, int templateID) =>
        SetSlot(slot, templateID, 1);

    public void SetSlot(short slot, int templateID, short count) =>
        SetSlot(slot, _manager.Retrieve(templateID).Result, count);

    public void SetSlot(short slot, IItemTemplate? template) =>
        SetSlot(slot, template, 1);

    public void SetSlot(short slot, IItemTemplate? template, short count)
    {
        var item = template?.ToItemSlot();

        if (item is IItemSlotBundle bundle)
            bundle.Number = count;
        if (item != null) SetSlot(slot, item);
    }

    public IItemSlot? TakeSlot(short slot) =>
        TakeSlot(slot, 1);

    public IItemSlot? TakeSlot(short slot, short count)
    {
        var item = this[slot];

        if (item is IItemSlotBundle bundle)
        {
            if (bundle.Number > count)
            {
                var newBundle = new ItemSlotBundle
                {
                    ID = bundle.ID,
                    DateExpire = bundle.DateExpire,
                    Number = bundle.Number,
                    Attribute = bundle.Attribute,
                    Title = bundle.Title
                };

                newBundle.Number = count;
                bundle.Number -= count;

                UpdateQuantitySlot(slot, bundle.Number);
                return newBundle;
            }

            bundle.Number = count;
        }

        RemoveSlot(slot);
        return item;
    }

    public void UpdateQuantitySlot(short slot, short quantity)
        => _operations.Enqueue(new UpdateQuantityModifyInventoryOperation(_type, slot, quantity));

    public void UpdateEXPSlot(short slot, int exp)
        => _operations.Enqueue(new UpdateEXPModifyInventoryOperation(_type, slot, exp));
}
