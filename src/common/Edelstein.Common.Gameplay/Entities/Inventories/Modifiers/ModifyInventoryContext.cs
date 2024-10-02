using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Edelstein.Protocol.Gameplay.Constants;
using Edelstein.Protocol.Gameplay.Entities.Inventories;
using Edelstein.Protocol.Gameplay.Entities.Inventories.Modifiers;
using Edelstein.Protocol.Gameplay.Entities.Inventories.Modifiers.Operations;
using Edelstein.Protocol.Gameplay.Entities.Inventories.Templates;
using Edelstein.Protocol.Utilities.Templates;

namespace Edelstein.Common.Gameplay.Entities.Inventories.Modifiers;

public class ModifyInventoryContext(
    ItemInventoryType type,
    ItemInventory inventory,
    ITemplateManager<IItemTemplate> templates
) : AbstractModifyInventory, IModifyInventoryContext
{
    public override Queue<StructuredModifyInventoryOperation> Operations { get; } = new();
    
    public ItemSlotBase? this[short slot] => inventory.Items.TryGetValue(slot, out var item)
        ? item
        : null;

    public void SetSlot(short slot, ItemSlotBase item)
    {
        inventory.Items[slot] = item;
        Operations.Enqueue(new StructuredModifyInventoryOperation
        {
            Type = ModifyInventoryOperationType.Add,
            Info = new StructuredModifyInventoryOperationInfoAdd
            {
                Item = item.ToStructured(),
                Inventory = type,
                Slot = slot
            }
        });
    }

    public void SetSlot(short slot, int templateID, short count = 1)
        => SetSlot(slot, templates.Retrieve(templateID).Result, count);

    public void SetSlot(short slot, IItemTemplate? template, short count = 1)
    {
        var item = template?.ToItemSlot();

        if (item is ItemSlotBundle bundle)
            bundle.Number = count;
        if (item != null) SetSlot(slot, item);
    }

    public ItemSlotBase? TakeSlot(short slot, short count = 1)
    {
        var item = this[slot];

        if (item is ItemSlotBundle bundle)
        {
            if (bundle.Number > count)
            {
                var newBundle = new ItemSlotBundle
                {
                    TemplateID = bundle.TemplateID,
                    DateExpire = bundle.DateExpire,
                    Number = bundle.Number,
                    Attribute = bundle.Attribute,
                    Title = bundle.Title
                };

                newBundle.Number = count;
                bundle.Number -= count;

                UpdateNumberSlot(slot, bundle.Number);
                return newBundle;
            }

            bundle.Number = count;
        }

        RemoveSlot(slot);
        return item;
    }

    public void RemoveSlot(short slot)
    {
        inventory.Items.Remove(slot);
        Operations.Enqueue(new StructuredModifyInventoryOperation
        {
            Type = ModifyInventoryOperationType.Remove,
            Info = new StructuredModifyInventoryOperationInfoRemove
            {
                Inventory = type,
                Slot = slot
            }
        });
    }

    public void MoveSlot(short from, short to)
    {
        var itemFrom = this[from];
        var itemTo = this[to];

        inventory.Items.Remove(from);
        inventory.Items.Remove(to);

        if (itemTo != null) inventory.Items[from] = itemTo;
        if (itemFrom != null) inventory.Items[to] = itemFrom;

        Operations.Enqueue(new StructuredModifyInventoryOperation
        {
            Type = ModifyInventoryOperationType.Move,
            Info = new StructuredModifyInventoryOperationInfoMove
            {
                Inventory = type,
                Slot = from,
                ToSlot = to
            }
        });
    }

    public void UpdateSlot(short slot)
    {
        var item = this[slot];
        if (item == null) return;
        
        Operations.Enqueue(new StructuredModifyInventoryOperation
        {
            Type = ModifyInventoryOperationType.Add,
            Info = new StructuredModifyInventoryOperationInfoAdd
            {
                Item = item.ToStructured(),
                Inventory = type,
                Slot = slot
            }
        });
    }
    
    public override short Add(ItemSlotBase item)
    {
        switch (item)
        {
            case ItemSlotBundle bundle:
                if (templates.Retrieve(bundle.TemplateID).Result is not IItemBundleTemplate template) goto default;
                if (bundle.Number < 1) bundle.Number = 1;

                var mergeable = inventory.Items
                    .Where(kv => kv.Value is ItemSlotBundle b && b.Number < template.MaxPerSlot)
                    .Select(kv => Tuple.Create(kv.Key, (ItemSlotBundle)kv.Value))
                    .FirstOrDefault(t => t.Item2.IsMergeableWith(bundle));

                if (mergeable != null)
                {
                    var count = bundle.Number + mergeable.Item2.Number;
                    var maxNumber = template.MaxPerSlot;

                    if (count > maxNumber)
                    {
                        var leftover = count - maxNumber;

                        bundle.Number = (short)leftover;
                        mergeable.Item2.Number = maxNumber;

                        UpdateNumberSlot(mergeable.Item1, mergeable.Item2.Number);
                        Add(bundle);
                        return mergeable.Item1;
                    }

                    mergeable.Item2.Number += bundle.Number;
                    UpdateNumberSlot(mergeable.Item1, mergeable.Item2.Number);
                    return mergeable.Item1;
                }

                goto default;
            default:
                var slot = Enumerable.Range(1, inventory.SlotMax)
                    .Select(i => (short)i)
                    .Except(inventory.Items.Keys)
                    .FirstOrDefault(i => i > 0);

                if (slot > 0) SetSlot(slot, item);
                return slot;
        }
    }

    public override short Add(int templateID, short count = 1)
        => Add(templates.Retrieve(templateID).Result, count);

    public override short Add(IItemTemplate? template, short count = 1)
    {
        var item = template?.ToItemSlot();

        if (item is ItemSlotBundle bundle)
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

    public override void Remove(int templateID, short count = 1)
    {
        var removed = 0;
        var match = inventory.Items
            .Where(kv => kv.Key > 0)
            .Where(kv => kv.Value.TemplateID == templateID)
            .ToImmutableArray();

        foreach (var kv in match)
        {
            if (removed >= count) return;
            if (kv.Value is ItemSlotBundle bundle)
            {
                var diff = count - removed;

                if (bundle.Number > diff)
                {
                    removed += diff;
                    bundle.Number = (short)(bundle.Number - diff);
                    UpdateNumberSlot(kv.Key, bundle.Number);
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

    public override void RemoveAll(int templateID)
    {
        var match = inventory.Items
            .Where(kv => kv.Value.TemplateID == templateID)
            .ToImmutableArray();

        foreach (var kv in match)
            RemoveSlot(kv.Key);
    }

    public void UpdateNumberSlot(short slot, short count)
        => Operations.Enqueue(new StructuredModifyInventoryOperation
        {
            Type = ModifyInventoryOperationType.UpdateNumber,
            Info = new StructuredModifyInventoryOperationInfoUpdateNumber
            {
                Inventory = type,
                Slot = slot,
                Number = count
            }
        });

    public void UpdateEXPSlot(short slot, int exp)
        => Operations.Enqueue(new StructuredModifyInventoryOperation
        {
            Type = ModifyInventoryOperationType.UpdateEXP,
            Info = new StructuredModifyInventoryOperationInfoUpdateEXP
            {
                Inventory = type,
                Slot = slot,
                EXP = exp
            }
        });

    public override void Gather()
    {
        var inventoryCopy = inventory.Items
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
        var inventoryCopy = inventory.Items
            .Where(kv => kv.Key > 0)
            .OrderBy(kv => kv.Value.TemplateID)
            .ThenByDescending(kv => kv.Value is ItemSlotBundle bundle ? bundle.Number : 1)
            .ToImmutableArray();

        foreach (var kv in inventoryCopy)
            RemoveSlot(kv.Key);
        foreach (var kv in inventoryCopy)
            Add(kv.Value);
    }

    public override void Clear()
        => inventory.Items
            .Where(kv => kv.Key > 0)
            .ToImmutableList()
            .ForEach(kv => RemoveSlot(kv.Key));
}
