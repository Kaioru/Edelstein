using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Edelstein.Common.Gameplay.Users.Inventories.Modify.Operations;
using Edelstein.Protocol.Gameplay.Templating;
using Edelstein.Protocol.Gameplay.Users.Inventories;
using Edelstein.Protocol.Gameplay.Users.Inventories.Modify;
using Edelstein.Protocol.Gameplay.Users.Inventories.Templates;
using Edelstein.Protocol.Network;
using Edelstein.Protocol.Network.Utils;
using MoreLinq;

namespace Edelstein.Common.Gameplay.Users.Inventories.Modify
{
    public class ModifyInventoryContext : IModifyInventoryContext, IPacketWritable
    {
        public IEnumerable<IModifyInventoryOperation> History => _history.ToImmutableList();

        private readonly Queue<AbstractModifyInventoryOperation> _history;
        private readonly ItemInventoryType _type;
        private readonly ItemInventory _inventory;
        private readonly ITemplateRepository<ItemTemplate> _templates;

        // TODO: exception throwing for various inventory operations
        public ModifyInventoryContext(
            ItemInventoryType type,
            ItemInventory inventory,
            ITemplateRepository<ItemTemplate> templates
        )
        {
            _history = new Queue<AbstractModifyInventoryOperation>();
            _type = type;
            _inventory = inventory;
            _templates = templates;
        }

        public void Add(AbstractItemSlot item)
        {
            switch (item)
            {
                case ItemSlotBundle bundle:
                    var template = _templates.Retrieve(bundle.TemplateID).Result as ItemBundleTemplate;

                    // if (ItemConstants.IsRechargeableItem(bundle.TemplateID)) goto default; // TODO: rechargeable constants
                    if (bundle.Number < 1) bundle.Number = 1;

                    var mergeable = _inventory.Items.Values
                        .OfType<ItemSlotBundle>()
                        .Where(b => bundle.Number + b.Number <= template.MaxPerSlot)
                        .FirstOrDefault(b => b.Equals(bundle));

                    if (mergeable != null)
                    {
                        var count = bundle.Number + mergeable.Number;
                        var maxNumber = template.MaxPerSlot;

                        if (count > maxNumber)
                        {
                            var leftover = count - maxNumber;

                            bundle.Number = (short)leftover;
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
                        .Select(i => (short)i)
                        .Except(_inventory.Items.Keys)
                        .First(i => i > 0);

                    Set(slot, item);
                    break;
            }
        }

        public void Add(int templateID, short quantity = 1)
            => Add(_templates.Retrieve(templateID).Result, quantity);

        public void Add(ItemTemplate template, short quantity = 1)
        {
            var item = template.ToItemSlot();

            if (item is ItemSlotBundle bundle)
            {
                bundle.Number = quantity;

                Add(item);
                return;
            }

            for (var i = 0; i < quantity; i++)
                Add(item);
        }

        public void Set(short slot, AbstractItemSlot item)
        {
            _inventory.Items[slot] = item;
            _history.Enqueue(new AddModifyInventoryOperation(_type, slot, item));
        }

        public void Set(short slot, int templateID, short quantity = 1)
            => Set(slot, _templates.Retrieve(templateID).Result, quantity);

        public void Set(short slot, ItemTemplate template, short quantity = 1)
        {

            var item = template.ToItemSlot();

            if (item is ItemSlotBundle bundle)
                bundle.Number = quantity;

            Set(slot, item);
        }

        public void Remove(short slot)
        {
            _inventory.Items.Remove(slot);
            _history.Enqueue(new RemoveModifyInventoryOperation(_type, slot));
        }

        public void Remove(short slot, short count)
        {

            if (_inventory.Items[slot] is ItemSlotBundle bundle) // TODO: recharageabl
            {
                if (count > 0)
                {
                    bundle.Number -= count;
                    bundle.Number = Math.Max((short)0, bundle.Number);

                    if (bundle.Number > 0)
                    {
                        UpdateQuantity(bundle);
                        return;
                    }
                }
            }

            Remove(slot);
        }

        public void Remove(int templateID, short count)
        {
            var removed = 0;

            _inventory.Items.Values
                .Where(i => i.TemplateID == templateID)
                .ForEach(i =>
                {
                    if (removed >= count) return;
                    if (i is ItemSlotBundle bundle) // TODO: recharagle
                    {
                        var diff = count - removed;

                        if (bundle.Number > diff)
                        {
                            removed += diff;
                            bundle.Number = (short)(bundle.Number - diff);

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

        public void Remove(AbstractItemSlot item)
            => Remove(_inventory.Items.First(i => i.Value == item).Key);

        public void Remove(AbstractItemSlot item, short count)
        {

            if (item is ItemSlotBundle bundle) // TODO: recharageable
            {
                if (count > 0)
                {
                    bundle.Number -= count;
                    bundle.Number = Math.Max((short)0, bundle.Number);

                    if (bundle.Number > 0)
                    {
                        UpdateQuantity(bundle);
                        return;
                    }
                }
            }

            Remove(item);
        }

        public void Move(short from, short to)
        {
            var item = _inventory.Items[from];

            if (
                item is ItemSlotBundle bundle && // TODO: recharegable
                _inventory.Items.ContainsKey(to) &&
                _inventory.Items[to] is ItemSlotBundle existing &&
                bundle.Equals(existing)
            )
            {
                var template = _templates.Retrieve(bundle.TemplateID).Result as ItemBundleTemplate;
                var count = bundle.Number + existing.Number;
                var maxNumber = template.MaxPerSlot;

                if (count > maxNumber)
                {
                    var leftover = count - maxNumber;

                    bundle.Number = (short)leftover;
                    existing.Number = maxNumber;

                    UpdateQuantity(bundle);
                }
                else
                {
                    existing.Number = (short)count;

                    Remove(bundle);
                }

                UpdateQuantity(existing);
                return;
            }

            _inventory.Items.Remove(from);

            if (_inventory.Items.ContainsKey(to))
                _inventory.Items[from] = _inventory.Items[to];
            _inventory.Items[to] = item;
            _history.Enqueue(new MoveModifyInventoryOperation(_type, from, to));
        }

        public ItemSlotBundle Take(short slot, short count = 1)
        {
            var bundle = (ItemSlotBundle)_inventory.Items[slot];
            var result = new ItemSlotBundle
            {
                TemplateID = bundle.TemplateID,
                Number = count,
                Attribute = bundle.Attribute,
                Title = bundle.Title,
                DateExpire = bundle.DateExpire,
                CashItemSN = bundle.CashItemSN
            };

            Remove(bundle, count);
            return result;
        }

        public ItemSlotBundle Take(int templateID, short count = 1)
            => Take(_inventory.Items.First(i => i.Value.TemplateID == templateID).Key, count);

        public ItemSlotBundle Take(ItemSlotBundle bundle, short count = 1)
            => Take(_inventory.Items.First(i => i.Value.Equals(bundle)).Key, count);

        public void Update(short slot)
        {
            var item = _inventory.Items[slot];

            Remove(slot);
            Set(slot, item);
        }

        public void Update(AbstractItemSlot item)
            => Update(_inventory.Items.First(i => i.Value == item).Key);

        private void UpdateQuantity(ItemSlotBundle item)
            => UpdateQuantity(_inventory.Items.First(i => i.Value == item).Key, item.Number);

        private void UpdateQuantity(short slot, short quantity)
            => _history.Enqueue(new UpdateQuantityModifyInventoryOperation(_type, slot, quantity));

        public void WriteToPacket(IPacketWriter writer)
        {
            writer.WriteByte((byte)_history.Count);
            _history.ForEach(o => writer.Write(o));
        }
    }
}
