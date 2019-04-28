using System;
using System.Collections.Generic;
using System.Linq;
using Edelstein.Core.Extensions.Templates;
using Edelstein.Core.Gameplay.Constants;
using Edelstein.Core.Gameplay.Inventories.Operations;
using Edelstein.Database.Entities;
using Edelstein.Database.Entities.Inventories;
using Edelstein.Database.Entities.Inventories.Items;
using Edelstein.Network.Packets;
using Edelstein.Provider.Templates.Item;
using MoreLinq.Extensions;

namespace Edelstein.Core.Gameplay.Inventories
{
    public class ModifyInventoryContext
    {
        private readonly ItemInventoryType _type;
        private readonly ItemInventory _inventory;
        private readonly Queue<AbstractModifyInventoryOperation> _operations;

        public ModifyInventoryContext(ItemInventoryType type, ItemInventory inventory)
        {
            _type = type;
            _inventory = inventory;
            _operations = new Queue<AbstractModifyInventoryOperation>();
        }

        public void Add(ItemSlot item)
        {
            switch (item)
            {
                case ItemSlotBundle bundle:
                    if (ItemConstants.IsRechargeableItem(bundle.TemplateID)) goto default;
                    if (bundle.Number < 1) bundle.Number = 1;
                    if (bundle.MaxNumber < 1) bundle.MaxNumber = 1;

                    var mergeable = _inventory.Items
                        .Where(i => i.Key > 0)
                        .Select(i => new {i.Key, i.Value})
                        .FirstOrDefault(i => i.Value is ItemSlotBundle b &&
                                             i.Key <= _inventory.SlotMax &&
                                             b.TemplateID == bundle.TemplateID &&
                                             b.Attribute == bundle.Attribute &&
                                             b.Title == bundle.Title &&
                                             b.DateExpire == bundle.DateExpire &&
                                             b.Number != b.MaxNumber);

                    if (mergeable != null)
                    {
                        var existing = (ItemSlotBundle) mergeable.Value;
                        var count = bundle.Number + existing.Number;
                        var maxNumber = existing.MaxNumber;

                        if (count > maxNumber)
                        {
                            var leftover = count - maxNumber;

                            bundle.Number = (short) leftover;
                            existing.Number = maxNumber;
                            UpdateQuantity(mergeable.Key);
                            Add(bundle);
                            return;
                        }

                        existing.Number += bundle.Number;
                        UpdateQuantity(mergeable.Key);
                        return;
                    }

                    goto default;
                default:
                    var usedSlots = _inventory.Items.Keys
                        .Where(s => s > 0)
                        .Where(s => s <= _inventory.SlotMax)
                        .ToList();
                    var unusedSlots = Enumerable.Range(1, _inventory.SlotMax)
                        .Cast<short>()
                        .Except(usedSlots)
                        .ToList();

                    if (unusedSlots.Count == 0) return;

                    Set(unusedSlots.First(), item);
                    break;
            }
        }

        public void Set(short slot, ItemTemplate template)
        {
            Set(slot, template.ToItemSlot());
        }

        public void Set(short slot, ItemSlot item)
        {
            if (_inventory.Items.ContainsKey(slot))
                Remove(slot);

            _inventory.Items.Add(slot, item);
            _operations.Enqueue(new AddInventoryOperation(_type, slot, item));
        }

        public void Remove(short slot)
        {
            _inventory.Items.Remove(slot);
            _operations.Enqueue(new RemoveInventoryOperation(_type, slot));
        }

        public void Remove(short slot, int count)
        {
            var item = _inventory.Items[slot];

            if (item is ItemSlotBundle bundle &&
                !ItemConstants.IsRechargeableItem(bundle.TemplateID))
            {
                if (count > 0)
                {
                    bundle.Number -= (short) count;
                    bundle.Number = Math.Max((short) 0, bundle.Number);

                    if (bundle.Number > 0)
                    {
                        UpdateQuantity(slot);
                        return;
                    }
                }
            }

            Remove(slot);
        }

        public void Remove(int templateID, int count = 1)
        {
            var removed = 0;

            _inventory.Items
                .Where(kv => kv.Value.TemplateID == templateID)
                .ForEach(kv =>
                {
                    if (removed >= count) return;
                    if (kv.Value is ItemSlotBundle bundle &&
                        !ItemConstants.IsRechargeableItem(bundle.TemplateID))
                    {
                        var diff = count - removed;

                        if (bundle.Number > diff)
                        {
                            removed += diff;
                            bundle.Number = (short) (bundle.Number - diff);
                            UpdateQuantity(kv.Key);
                        }
                        else
                        {
                            removed += bundle.Number;
                            Remove(kv.Key);
                        }
                    }
                    else
                    {
                        removed++;
                        Remove(kv.Key);
                    }
                });
        }

        public ItemSlotBundle Take(short slot, short count = 1)
        {
            var bundle = (ItemSlotBundle) _inventory.Items.First(kv => kv.Key == slot && kv.Value is ItemSlotBundle).Value;
            var result = new ItemSlotBundle
            {
                TemplateID = bundle.TemplateID,
                DateExpire = bundle.DateExpire,
                Number = count,
                MaxNumber = bundle.MaxNumber,
                Attribute = bundle.Attribute,
                Title = bundle.Title
            };

            Remove(slot, count);
            return result;
        }

        public void Move(short from, short to)
        {
            var item = _inventory.Items[from];

            if (_inventory.Items.ContainsKey(to))
            {
                if (
                    !ItemConstants.IsRechargeableItem(item.TemplateID) &&
                    item is ItemSlotBundle bundle &&
                    _inventory.Items[to] is ItemSlotBundle target)
                {
                    if (bundle.TemplateID == target.TemplateID &&
                        bundle.Attribute == target.Attribute &&
                        bundle.Title == target.Title &&
                        bundle.DateExpire == target.DateExpire &&
                        bundle.MaxNumber == target.MaxNumber)
                    {
                        var count = bundle.Number + target.Number;
                        var maxNumber = target.MaxNumber;

                        if (count > maxNumber)
                        {
                            var leftover = count - maxNumber;

                            bundle.Number = (short) leftover;
                            target.Number = maxNumber;
                            UpdateQuantity(from);
                        }
                        else
                        {
                            target.Number = (short) count;
                            Remove(from);
                        }

                        UpdateQuantity(to);
                        return;
                    }
                }
            }

            _inventory.Items[to] = item;
            _inventory.Items.Remove(from);
            _operations.Enqueue(new MoveInventoryOperation(_type, from, to));
        }

        public void Update(short slot)
        {
            _operations.Enqueue(new RemoveInventoryOperation(_type, slot));
            _operations.Enqueue(new AddInventoryOperation(_type, slot, _inventory.Items[slot]));
        }

        private void UpdateQuantity(short slot)
        {
            _operations.Enqueue(new UpdateQuantityInventoryOperation(
                _type,
                slot,
                ((ItemSlotBundle) _inventory.Items[slot]).Number)
            );
        }

        public void Encode(IPacket packet)
        {
            packet.Encode<byte>((byte) _operations.Count);
            _operations.ForEach(o => o.Encode(packet));
        }
    }
}