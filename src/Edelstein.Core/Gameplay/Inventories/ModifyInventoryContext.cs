using System.Collections.Generic;
using Edelstein.Core.Extensions.Templates;
using Edelstein.Core.Gameplay.Constants;
using Edelstein.Core.Gameplay.Inventories.Operations;
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
                        bundle.DateExpire == target.DateExpire)
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