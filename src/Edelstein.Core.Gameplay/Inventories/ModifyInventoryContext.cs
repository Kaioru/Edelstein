using System;
using System.Collections.Generic;
using System.Linq;
using Edelstein.Core.Gameplay.Constants;
using Edelstein.Core.Gameplay.Inventories.Operations;
using Edelstein.Core.Templates.Items;
using Edelstein.Entities.Inventories;
using Edelstein.Entities.Inventories.Items;
using Edelstein.Network.Packets;
using MoreLinq;

namespace Edelstein.Core.Gameplay.Inventories
{
    public class ModifyInventoryContext : IModifyInventoryContext
    {
        private readonly ItemInventoryType _type;
        private readonly ItemInventory _inventory;

        public Queue<AbstractModifyInventoryOperation> Operations { get; }

        public ModifyInventoryContext(ItemInventoryType type, ItemInventory inventory)
        {
            _type = type;
            _inventory = inventory;

            Operations = new Queue<AbstractModifyInventoryOperation>();
        }

        public ModifyInventoryContext(ItemInventory inventory)
        {
            _type = ItemInventoryType.Equip;
            _inventory = inventory;

            Operations = new Queue<AbstractModifyInventoryOperation>();
        }

        public void Add(ItemSlot item)
        {
            switch (item)
            {
                case ItemSlotBundle bundle:
                    if (ItemConstants.IsRechargeableItem(bundle.TemplateID)) goto default;
                    if (bundle.Number < 1) bundle.Number = 1;
                    if (bundle.MaxNumber < 1) bundle.MaxNumber = 1;

                    var mergeable = _inventory.Items.Values
                        .OfType<ItemSlotBundle>()
                        .Where(b => bundle.Number + b.Number <= b.MaxNumber)
                        .FirstOrDefault(b => b.Equals(bundle));

                    if (mergeable != null)
                    {
                        var count = bundle.Number + mergeable.Number;
                        var maxNumber = mergeable.MaxNumber;

                        if (count > maxNumber)
                        {
                            var leftover = count - maxNumber;

                            bundle.Number = (short) leftover;
                            mergeable.Number = maxNumber;
                            UpdateQuantity(mergeable);
                            Add(bundle);
                            return;
                        }

                        mergeable.Number += bundle.Number;
                        UpdateQuantity(mergeable);
                        return;
                    }

                    goto default;
                default:
                    var slot = Enumerable.Range(1, _inventory.SlotMax)
                        .Select(i => (short) i)
                        .Except(_inventory.Items.Keys)
                        .First(i => i > 0);

                    Set(slot, item);
                    break;
            }
        }

        public void Add(ItemTemplate template, short quantity = 1)
        {
            var item = template.ToItemSlot();

            if (item is ItemSlotBundle b)
                b.Number = quantity;
            Add(item);
        }

        public void Set(short slot, ItemSlot item)
        {
            _inventory.Items[slot] = item;
            Operations.Enqueue(new AddInventoryOperation(_type, slot, item));
        }

        public void Set(short slot, ItemTemplate template, short quantity = 1)
        {
            var item = template.ToItemSlot();

            if (item is ItemSlotBundle b)
                b.Number = quantity;
            Set(slot, item);
        }

        public void Set(BodyPart part, ItemSlot item)
            => Set((short) -(short) part, item);

        public void Set(BodyPart part, ItemTemplate template, short quantity = 1)
            => Set((short) -(short) part, template, quantity);

        public void Remove(short slot)
        {
            _inventory.Items.Remove(slot);
            Operations.Enqueue(new RemoveInventoryOperation(_type, slot));
        }

        public void Remove(short slot, short count)
        {
            if (_inventory.Items[slot] is ItemSlotBundle bundle &&
                !ItemConstants.IsRechargeableItem(bundle.TemplateID))
            {
                if (count > 0)
                {
                    bundle.Number -= count;
                    bundle.Number = Math.Max((short) 0, bundle.Number);

                    if (bundle.Number > 0)
                    {
                        UpdateQuantity(bundle);
                        return;
                    }
                }
            }

            Remove(slot);
        }

        public void Remove(ItemSlot slot)
        {
            Remove(_inventory.Items.First(i => i.Value == slot).Key);
        }

        public void Remove(ItemSlot item, short count)
        {
            if (item is ItemSlotBundle bundle &&
                !ItemConstants.IsRechargeableItem(bundle.TemplateID))
            {
                if (count > 0)
                {
                    bundle.Number -= count;
                    bundle.Number = Math.Max((short) 0, bundle.Number);

                    if (bundle.Number > 0)
                    {
                        UpdateQuantity(bundle);
                        return;
                    }
                }
            }

            Remove(item);
        }

        public void Remove(int template, short count)
        {
            var removed = 0;

            _inventory.Items.Values
                .Where(i => i.TemplateID == template)
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

        public void Move(short from, short to)
        {
            var item = _inventory.Items[from];

            if (
                !ItemConstants.IsRechargeableItem(item.TemplateID) &&
                item is ItemSlotBundle bundle &&
                _inventory.Items.ContainsKey(to) &&
                _inventory.Items[to] is ItemSlotBundle existing &&
                bundle.Equals(existing)
            )
            {
                var count = bundle.Number + existing.Number;
                var maxNumber = existing.MaxNumber;

                if (count > maxNumber)
                {
                    var leftover = count - maxNumber;

                    bundle.Number = (short) leftover;
                    existing.Number = maxNumber;
                    UpdateQuantity(bundle);
                }
                else
                {
                    existing.Number = (short) count;
                    Remove(bundle);
                }

                UpdateQuantity(existing);
                return;
            }

            _inventory.Items.Remove(from);

            if (_inventory.Items.ContainsKey(to))
                _inventory.Items[from] = _inventory.Items[to];
            _inventory.Items[to] = item;
            Operations.Enqueue(new MoveInventoryOperation(_type, from, to));
        }

        public ItemSlotBundle Take(short slot, short count = 1)
        {
            var bundle = (ItemSlotBundle) _inventory.Items[slot];

            var result = new ItemSlotBundle
            {
                TemplateID = bundle.TemplateID,
                Number = count,
                MaxNumber = bundle.MaxNumber,
                Attribute = bundle.Attribute,
                Title = bundle.Title,
                DateExpire = bundle.DateExpire,
                CashItemSN = bundle.CashItemSN
            };

            Remove(bundle, count);
            return result;
        }

        public ItemSlotBundle Take(ItemSlotBundle bundle, short count = 1)
        {
            return Take(_inventory.Items.First(i => i.Value.Equals(bundle)).Key, count);
        }

        public ItemSlotBundle Take(int template, short count = 1)
        {
            return Take(_inventory.Items.First(i => i.Value.TemplateID == template).Key, count);
        }

        public void Update(short slot)
        {
            var item = _inventory.Items[slot];

            Remove(slot);
            Set(slot, item);
        }

        public void Update(ItemSlot slot)
        {
            Update(_inventory.Items.First(i => i.Value == slot).Key);
        }

        public void Encode(IPacket packet)
        {
            packet.Encode<byte>((byte) Operations.Count);
            Operations.ForEach(o => o.Encode(packet));
        }

        private void UpdateQuantity(ItemSlot slot)
        {
            UpdateQuantity(_inventory.Items.First(i => i.Value == slot).Key);
        }

        private void UpdateQuantity(short slot)
        {
            Operations.Enqueue(new UpdateQuantityInventoryOperation(
                _type,
                slot,
                ((ItemSlotBundle) _inventory.Items[slot]).Number
            ));
        }
    }
}