using System;
using System.Collections.Generic;
using System.Linq;
using Edelstein.Core.Constants;
using Edelstein.Core.Extensions;
using Edelstein.Core.Inventories.Exceptions;
using Edelstein.Core.Inventories.Operations;
using Edelstein.Data.Entities;
using Edelstein.Data.Entities.Inventory;
using Edelstein.Network.Packet;
using Edelstein.Provider.Templates.Item;
using MoreLinq;

namespace Edelstein.Core.Inventories
{
    public class ModifyInventoryContext
    {
        private readonly Character _character;
        private readonly Queue<AbstractInventoryOperation> _operations;

        public ModifyInventoryContext(Character character)
        {
            _character = character;
            _operations = new Queue<AbstractInventoryOperation>();
        }

        public void Add(ItemInventoryType type, ItemSlot item)
        {
            var inventory = _character.GetInventory(type);

            switch (item)
            {
                case ItemSlotBundle bundle:
                    if (ItemConstants.IsRechargeableItem(bundle.TemplateID)) goto default;
                    if (bundle.Number < 1) bundle.Number = 1;
                    if (bundle.MaxNumber < 1) bundle.MaxNumber = 1;

                    var mergeableSlots = inventory.Items
                        .OfType<ItemSlotBundle>()
                        .Where(b => b.TemplateID == bundle.TemplateID)
                        .Where(b => b.DateExpire == bundle.DateExpire)
                        .Where(b => b.Attribute == bundle.Attribute)
                        .Where(b => b.Title == bundle.Title)
                        .Where(b => b.Number != b.MaxNumber)
                        .Where(b => b.Position > 0)
                        .Where(b => b.Position <= inventory.SlotMax)
                        .ToList();

                    if (mergeableSlots.Count > 0)
                    {
                        var existingBundle = mergeableSlots.First();

                        var count = bundle.Number + existingBundle.Number;
                        var maxNumber = existingBundle.MaxNumber;

                        if (count > maxNumber)
                        {
                            var leftover = count - maxNumber;

                            bundle.Number = (short) leftover;
                            existingBundle.Number = maxNumber;
                            UpdateQuantity(existingBundle);
                            Add(bundle);
                            return;
                        }

                        existingBundle.Number += bundle.Number;
                        UpdateQuantity(existingBundle);
                        return;
                    }

                    goto default;
                default:
                    var usedSlots = inventory.Items
                        .Select<ItemSlot, int>(i => i.Position)
                        .Where(s => s > 0)
                        .Where(s => s <= inventory.SlotMax)
                        .ToList();
                    var unusedSlots = Enumerable.Range(1, inventory.SlotMax)
                        .Except(usedSlots)
                        .ToList();

                    if (unusedSlots.Count == 0) throw new InventoryFullException();

                    Set(type, item, (short) unusedSlots.First());
                    break;
            }
        }

        public void Add(ItemSlot item)
        {
            Add(item.ItemInventory?.Type ?? (ItemInventoryType) (item.TemplateID / 1000000), item);
        }

        public void Add(ItemTemplate template, short quantity = 1, ItemVariationType type = ItemVariationType.None)
        {
            var item = template.ToItemSlot();

            if (item is ItemSlotBundle bundle &&
                !ItemConstants.IsRechargeableItem(bundle.TemplateID))
            {
                bundle.Number = quantity;
                Add(bundle);
            }
            else
                for (var i = 0; i < quantity; i++)
                    Add(template.ToItemSlot(type));
        }

        public void Set(ItemTemplate template, short slot)
        {
            Set((ItemInventoryType) (template.ID / 1000000), template.ToItemSlot(), slot);
        }

        public void Set(ItemInventoryType type, ItemSlot item, short slot)
        {
            item.ItemInventory = _character.GetInventory(type);
            item.Position = slot;
            Set(item);
        }

        public void Set(ItemSlot item)
        {
            var inventory = item.ItemInventory;
            var inventoryItems = inventory.Items;
            var existingItem = inventoryItems.SingleOrDefault(i => i.Position == item.Position);

            if (existingItem != null) Remove(existingItem);

            inventoryItems.Add(item);
            _operations.Enqueue(new AddInventoryOperation(
                    inventory.Type,
                    item.Position,
                    item
                )
            );
        }

        public void Remove(ItemInventoryType type, short slot, int count = 1)
        {
            var inventory = _character.GetInventory(type);
            var inventoryItems = inventory.Items;
            var item = inventoryItems.SingleOrDefault(i => i.Position == slot);

            if (item != null) Remove(item, count);
        }

        public void Remove(ItemSlot item, int count = 0)
        {
            var inventory = item.ItemInventory;
            var inventoryItems = inventory.Items;

            item.ID = 0;
            item.ItemInventory = null;

            if (item is ItemSlotBundle bundle &&
                !ItemConstants.IsRechargeableItem(bundle.TemplateID))
            {
                if (count > 0)
                {
                    bundle.Number -= (short) count;
                    bundle.Number = Math.Max((short) 0, bundle.Number);

                    if (bundle.Number > 0)
                    {
                        UpdateQuantity(bundle);
                        return;
                    }
                }
            }

            inventoryItems.Remove(item);
            _operations.Enqueue(new RemoveInventoryOperation(
                inventory.Type,
                item.Position)
            );
        }

        public void Remove(int templateID, int count = 1)
        {
            Remove((ItemInventoryType) (templateID / 1000000), templateID, count);
        }

        public void Remove(ItemInventoryType type, int templateID, int count = 1)
        {
            var inventory = _character.GetInventory(type);
            var items = inventory.Items;
            var removed = 0;

            items.ToList()
                .Where(i => i.TemplateID == templateID)
                .ForEach(i =>
                {
                    if (removed >= count) return;
                    if (i is ItemSlotBundle bundle &&
                        !ItemConstants.IsRechargeableItem(bundle.TemplateID))
                    {
                        var diff = count - removed;

                        if (bundle.Number > diff)
                        {
                            removed += diff;
                            bundle.Number = (short) (bundle.Number - diff);
                            UpdateQuantity(bundle);
                        }
                        else
                        {
                            removed += bundle.Number;
                            Remove(bundle);
                        }
                    }
                    else
                    {
                        removed++;
                        Remove(i);
                    }
                });
        }

        public void Move(ItemInventoryType type, short fromSlot, short toSlot)
        {
            var inventory = _character.GetInventory(type);
            var inventoryItems = inventory.Items;
            var item = inventoryItems.SingleOrDefault(i => i.Position == fromSlot);

            if (item != null) Move(item, toSlot);
        }

        public void Move(ItemSlot item, short toSlot)
        {
            var inventory = item.ItemInventory;
            var existingItem = inventory.Items.SingleOrDefault(i => i.Position == toSlot);
            var fromSlot = item.Position;

            if (item is ItemSlotBundle bundle &&
                !ItemConstants.IsRechargeableItem(bundle.TemplateID))
            {
                if (existingItem is ItemSlotBundle existingBundle)
                {
                    if (bundle.TemplateID == existingBundle.TemplateID &&
                        bundle.Attribute == existingBundle.Attribute &&
                        bundle.Title == existingBundle.Title)
                    {
                        var count = bundle.Number + existingBundle.Number;
                        var maxNumber = existingBundle.MaxNumber;

                        if (count > maxNumber)
                        {
                            var leftover = count - maxNumber;

                            bundle.Number = (short) leftover;
                            existingBundle.Number = maxNumber;
                            UpdateQuantity(bundle);
                        }
                        else
                        {
                            existingBundle.Number = (short) count;
                            Remove(bundle);
                        }

                        UpdateQuantity(existingBundle);
                        return;
                    }
                }
            }

            if (existingItem != null)
            {
                existingItem.ItemInventory = inventory;
                existingItem.Position = fromSlot;
            }

            item.Position = toSlot;

            _operations.Enqueue(new MoveInventoryOperation(
                inventory.Type,
                fromSlot,
                toSlot)
            );
        }

        public void UpdateQuantity(ItemSlotBundle bundle)
        {
            _operations.Enqueue(new UpdateQuantityInventoryOperation(
                bundle.ItemInventory.Type,
                bundle.Position,
                bundle.Number)
            );
        }

        public void Update(ItemSlot slot)
        {
            _operations.Enqueue(new RemoveInventoryOperation(slot.ItemInventory.Type, slot.Position));
            _operations.Enqueue(new AddInventoryOperation(slot.ItemInventory.Type, slot.Position, slot));
        }

        public void Encode(IPacket packet)
        {
            packet.Encode<byte>((byte) _operations.Count);
            _operations.ForEach(o => o.Encode(packet));
        }
    }
}