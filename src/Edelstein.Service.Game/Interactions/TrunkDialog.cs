using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CSharpx;
using Edelstein.Core.Constants;
using Edelstein.Core.Extensions;
using Edelstein.Core.Services;
using Edelstein.Data.Entities.Inventory;
using Edelstein.Network.Packet;
using Edelstein.Service.Game.Fields.User;

namespace Edelstein.Service.Game.Interactions
{
    public class TrunkDialog : IDialog
    {
        private readonly int _npcTemplateID;
        private readonly ItemTrunk _trunk;
        private readonly int _getFee;
        private readonly int _putFee;

        public TrunkDialog(
            int npcTemplateID,
            ItemTrunk trunk,
            int getFee = 0,
            int putFee = 0
        )
        {
            _npcTemplateID = npcTemplateID;
            _trunk = trunk;
            _getFee = getFee;
            _putFee = putFee;
        }

        public async Task<bool> Enter(FieldUser user)
        {
            using (var p = new Packet(SendPacketOperations.TrunkResult))
            {
                p.Encode<byte>((byte) TrunkResult.OpenTrunkDlg);
                p.Encode<int>(_npcTemplateID);
                EncodeData(user, p);

                await user.SendPacket(p);
                return true;
            }
        }

        public Task OnPacket(RecvPacketOperations operation, FieldUser user, IPacket packet)
        {
            var type = (TrunkRequest) packet.Decode<byte>();

            switch (type)
            {
                case TrunkRequest.GetItem:
                    return OnTrunkGetItemRequest(user, packet);
                case TrunkRequest.PutItem:
                    return OnTrunkPutItemRequest(user, packet);
                case TrunkRequest.SortItem:
                    return OnTrunkSortItemRequest(user, packet);
                case TrunkRequest.Money:
                    return OnTrunkMoneyRequest(user, packet);
                case TrunkRequest.CloseDialog:
                    return user.Interact(this, true);
            }

            return Task.CompletedTask;
        }

        private async Task OnTrunkGetItemRequest(FieldUser user, IPacket packet)
        {
            packet.Decode<byte>();
            var pos = packet.Decode<byte>();
            var item = _trunk.Items.ToList()[pos];

            using (var p = new Packet(SendPacketOperations.TrunkResult))
            {
                var result = TrunkResult.GetSuccess;

                if (item == null) result = TrunkResult.GetUnknown;
                if (user.Character.Money < _getFee) result = TrunkResult.GetNoMoney;
                if (!user.Character.HasSlotFor(item)) result = TrunkResult.GetHavingOnlyItem;

                p.Encode<byte>((byte) result);

                if (result == TrunkResult.GetSuccess)
                {
                    item.ID = 0;
                    item.ItemTrunk = null;
                    _trunk.Items.Remove(item);
                    await user.ModifyStats(s => s.Money -= _getFee);
                    await user.ModifyInventory(i => i.Add(item));
                    EncodeData(user, p);
                }

                await user.SendPacket(p);
            }
        }

        private async Task OnTrunkPutItemRequest(FieldUser user, IPacket packet)
        {
            var pos = packet.Decode<short>();
            var templateID = packet.Decode<int>();
            var number = packet.Decode<short>();
            var inventory = user.Character.GetInventory((ItemInventoryType) (templateID / 1000000));
            var item = inventory.Items.FirstOrDefault(i => i.Position == pos);

            using (var p = new Packet(SendPacketOperations.TrunkResult))
            {
                var result = TrunkResult.PutSuccess;

                switch (item)
                {
                    case null:
                    case ItemSlotBundle bundle when bundle.Number < number:
                        result = TrunkResult.PutUnknown;
                        break;
                }

                if (user.Character.Money < _putFee) result = TrunkResult.GetNoMoney;
                if (_trunk.Items.Count >= _trunk.SlotMax) result = TrunkResult.PutNoSpace;

                p.Encode<byte>((byte) result);

                if (result == TrunkResult.PutSuccess)
                {
                    await user.ModifyStats(s => s.Money -= _putFee);
                    await user.ModifyInventory(i =>
                    {
                        if (!ItemConstants.IsTreatSingly(item.TemplateID))
                        {
                            if (!(item is ItemSlotBundle bundle)) return;
                            if (bundle.Number < number) return;

                            item = i.Take(bundle, number);
                        } else i.Remove(item);
                    });
                    _trunk.Items.Add(item);
                    EncodeData(user, p);
                }

                await user.SendPacket(p);
            }
        }

        private async Task OnTrunkSortItemRequest(FieldUser user, IPacket packet)
        {
            using (var p = new Packet(SendPacketOperations.TrunkResult))
            {
                p.Encode<byte>((byte) TrunkResult.SortItem);
                EncodeData(user, p);
                await user.SendPacket(p);
            }
        }

        private async Task OnTrunkMoneyRequest(FieldUser user, IPacket packet)
        {
            var amount = packet.Decode<int>();

            using (var p = new Packet(SendPacketOperations.TrunkResult))
            {
                var result = TrunkResult.MoneySuccess;

                if (amount < 0)
                {
                    amount = -amount;
                    if (amount > int.MaxValue - _trunk.Money)
                        result = TrunkResult.MoneyUnknown;
                    else if (user.Character.Money >= amount)
                    {
                        await user.ModifyStats(s => s.Money -= amount);
                        _trunk.Money += amount;
                    }
                    else result = TrunkResult.PutNoMoney;
                }
                else
                {
                    if (amount > int.MaxValue - user.Character.Money)
                        result = TrunkResult.MoneyUnknown;
                    if (_trunk.Money >= amount)
                    {
                        _trunk.Money -= amount;
                        await user.ModifyStats(s => s.Money += amount);
                    }
                    else result = TrunkResult.PutNoMoney;
                }

                p.Encode<byte>((byte) result);
                if (result == TrunkResult.MoneySuccess) EncodeData(user, p);

                await user.SendPacket(p);
            }
        }

        private void EncodeData(FieldUser user, IPacket packet)
        {
            long flag = -1;

            packet.Encode<byte>(_trunk.SlotMax);
            packet.Encode<long>(flag);

            void EncodeItems(ICollection<ItemSlot> items)
            {
                packet.Encode<byte>((byte) items.Count);
                items.ForEach(i =>
                {
                    if (i is ItemSlotEquip equip) equip.Encode(packet);
                    if (i is ItemSlotBundle bundle) bundle.Encode(packet);
                    if (i is ItemSlotPet pet) pet.Encode(packet);
                });
            }

            if ((flag & 0x2) != 0) packet.Encode<int>(_trunk.Money);
            if ((flag & 0x4) != 0) EncodeItems(_trunk.Items.Where(i => i.TemplateID / 1000000 == 1).ToList());
            if ((flag & 0x8) != 0) EncodeItems(_trunk.Items.Where(i => i.TemplateID / 1000000 == 2).ToList());
            if ((flag & 0x10) != 0) EncodeItems(_trunk.Items.Where(i => i.TemplateID / 1000000 == 3).ToList());
            if ((flag & 0x20) != 0) EncodeItems(_trunk.Items.Where(i => i.TemplateID / 1000000 == 4).ToList());
            if ((flag & 0x40) != 0) EncodeItems(_trunk.Items.Where(i => i.TemplateID / 1000000 == 5).ToList());
        }
    }
}