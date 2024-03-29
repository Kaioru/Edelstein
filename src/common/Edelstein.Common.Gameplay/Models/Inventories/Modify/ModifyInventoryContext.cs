﻿using System.Collections.Immutable;
using Edelstein.Common.Constants;
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

    public override short Add(IItemSlot? item)
    {
        switch (item)
        {
            case ItemSlotBundle bundle:
                if (_manager.Retrieve(bundle.ID).Result is not IItemBundleTemplate template) goto default;
                if (ItemConstants.IsRechargeableItem(template.ID)) goto default;
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
                        return mergeable.Item1;
                    }

                    mergeable.Item2.Number += bundle.Number;
                    UpdateQuantitySlot(mergeable.Item1, mergeable.Item2.Number);
                    return mergeable.Item1;
                }

                goto default;
            default:
                var slot = Enumerable.Range(1, _inventory.SlotMax)
                    .Select(i => (short)i)
                    .Except(_inventory.Items.Keys)
                    .FirstOrDefault(i => i > 0);

                if (slot > 0 && item != null) SetSlot(slot, item);
                return slot;
        }
    }

    public override void Remove(int templateID) =>
        Remove(templateID, 1);

    public override void Remove(int templateID, short count)
    {
        var removed = 0;
        var match = _inventory.Items
            .Where(kv => kv.Key > 0)
            .Where(kv => kv.Value.ID == templateID)
            .ToImmutableArray();

        foreach (var kv in match)
        {
            if (removed >= count) return;
            if (kv.Value is ItemSlotBundle bundle && !ItemConstants.IsRechargeableItem(bundle.ID))
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
            .ToImmutableArray();

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
            .ToImmutableArray();
        short position = 1;

        foreach (var kv in inventoryCopy)
            RemoveSlot(kv.Key);
        foreach (var kv in inventoryCopy)
            SetSlot(position++, kv.Value);
    }

    public override void Sort()
    {
        var inventoryCopy = _inventory.Items
            .Where(kv => kv.Key > 0)
            .OrderBy(kv => kv.Value.ID)
            .ThenByDescending(kv => kv.Value is IItemSlotBundle bundle ? bundle.Number : 1)
            .ToImmutableArray();

        foreach (var kv in inventoryCopy)
            RemoveSlot(kv.Key);
        foreach (var kv in inventoryCopy)
            Add(kv.Value);
    }
    public override void Clear() =>
        _inventory.Items
            .Where(kv => kv.Key > 0)
            .ToImmutableList()
            .ForEach(kv => RemoveSlot(kv.Key));

    public override short Add(int templateID) =>
        Add(templateID, 1);

    public override short Add(int templateID, short count) =>
        Add(_manager.Retrieve(templateID).Result, count);

    public override short Add(IItemTemplate? template) =>
        Add(template, 1);

    public override short Add(IItemTemplate? template, short count)
    {
        var item = template?.ToItemSlot();

        if (item is IItemSlotBundle bundle)
        {
            bundle.Number = count;

            if (template is IItemBundleTemplate bundleTemplate)
            {
                while (bundle.Number > bundleTemplate.MaxPerSlot)
                {
                    var reduce = (short)Math.Min(bundleTemplate.MaxPerSlot, bundle.Number - bundleTemplate.MaxPerSlot);

                    bundle.Number -= reduce;
                    Add(template, reduce);
                }
            }
        }
        if (item != null) 
            return Add(item);
        return -1;
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

        if (item is IItemSlotBundle bundle && !ItemConstants.IsRechargeableItem(bundle.ID))
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
