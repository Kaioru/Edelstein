using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CSharpx;
using Edelstein.Core.Extensions;
using Edelstein.Core.Services;
using Edelstein.Data.Entities.Inventory;
using Edelstein.Network.Packet;
using Edelstein.Service.Game.Field.User;

namespace Edelstein.Service.Game.Interactions
{
    public class TrunkDialog : IDialog
    {
        private readonly int _npcTemplateID;
        private readonly FieldUser _user;
        private readonly ItemTrunk _trunk;
        private readonly int _getFee;
        private readonly int _putFee;

        public TrunkDialog(
            int npcTemplateID,
            FieldUser user,
            ItemTrunk trunk,
            int getFee = 0,
            int putFee = 0
        )
        {
            _npcTemplateID = npcTemplateID;
            _user = user;
            _trunk = trunk;
            _getFee = getFee;
            _putFee = putFee;
        }

        public Task OnPacket(RecvPacketOperations operation, IPacket packet)
        {
            var type = (TrunkRequest) packet.Decode<byte>();

            switch (type)
            {
                case TrunkRequest.GetItem:
                    return OnTrunkGetItemRequest(packet);
                case TrunkRequest.PutItem:
                    return OnTrunkPutItemRequest(packet);
                case TrunkRequest.SortItem:
                    return OnTrunkSortItemRequest(packet);
                case TrunkRequest.Money:
                    return OnTrunkMoneyRequest(packet);
                case TrunkRequest.CloseDialog:
                    return _user.Interact(this, true);
            }

            return Task.CompletedTask;
        }

        private async Task OnTrunkGetItemRequest(IPacket packet)
        {
            packet.Decode<byte>();
            var pos = packet.Decode<byte>();
            var item = _trunk.Items.ToList()[pos];

            using (var p = new Packet(SendPacketOperations.TrunkResult))
            {
                var result = TrunkResult.GetSuccess;

                if (item == null) result = TrunkResult.GetUnknown;
                if (_user.Character.Money < _getFee) result = TrunkResult.GetNoMoney;
                if (!_user.Character.HasSlotFor(item)) result = TrunkResult.GetHavingOnlyItem;

                p.Encode<byte>((byte) result);

                if (result == TrunkResult.GetSuccess)
                {
                    item.ID = 0;
                    item.ItemTrunk = null;
                    _trunk.Items.Remove(item);
                    await _user.ModifyStats(s => s.Money -= _getFee);
                    await _user.ModifyInventory(i => i.Add(item));
                    EncodeData(p);
                }

                await _user.SendPacket(p);
            }
        }

        private async Task OnTrunkPutItemRequest(IPacket packet)
        {
            var pos = packet.Decode<short>();
            var templateID = packet.Decode<int>();
            var inventory = _user.Character.GetInventory((ItemInventoryType) (templateID / 1000000));
            var item = inventory.Items.FirstOrDefault(i => i.Position == pos);

            using (var p = new Packet(SendPacketOperations.TrunkResult))
            {
                var result = TrunkResult.PutSuccess;

                if (item == null) result = TrunkResult.PutUnknown;
                if (_user.Character.Money < _putFee) result = TrunkResult.GetNoMoney;
                if (_trunk.Items.Count >= _trunk.SlotMax) result = TrunkResult.PutNoSpace;

                p.Encode<byte>((byte) result);

                if (result == TrunkResult.PutSuccess)
                {
                    await _user.ModifyStats(s => s.Money -= _putFee);
                    await _user.ModifyInventory(i => i.Remove(item));
                    _trunk.Items.Add(item);
                    EncodeData(p);
                }

                await _user.SendPacket(p);
            }
        }

        private async Task OnTrunkSortItemRequest(IPacket packet)
        {
            using (var p = new Packet(SendPacketOperations.TrunkResult))
            {
                p.Encode<byte>((byte) TrunkResult.SortItem);
                EncodeData(p);
                await _user.SendPacket(p);
            }
        }

        private async Task OnTrunkMoneyRequest(IPacket packet)
        {
            int amount = packet.Decode<int>();

            using (var p = new Packet(SendPacketOperations.TrunkResult))
            {
                var result = TrunkResult.MoneySuccess;

                if (amount < 0)
                {
                    amount = -amount;
                    if (amount > int.MaxValue - _trunk.Money)
                        result = TrunkResult.MoneyUnknown;
                    else if (_user.Character.Money >= amount)
                    {
                        await _user.ModifyStats(s => s.Money -= amount);
                        _trunk.Money += amount;
                    }
                    else result = TrunkResult.PutNoMoney;
                }
                else
                {
                    if (amount > int.MaxValue - _user.Character.Money)
                        result = TrunkResult.MoneyUnknown;
                    if (_trunk.Money >= amount)
                    {
                        _trunk.Money -= amount;
                        await _user.ModifyStats(s => s.Money += amount);
                    }
                    else result = TrunkResult.PutNoMoney;
                }

                p.Encode<byte>((byte) result);
                if (result == TrunkResult.MoneySuccess) EncodeData(p);

                await _user.SendPacket(p);
            }
        }

        private void EncodeData(IPacket packet)
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

        public IPacket GetStartDialoguePacket()
        {
            using (var p = new Packet(SendPacketOperations.TrunkResult))
            {
                p.Encode<byte>((byte) TrunkResult.OpenTrunkDlg);
                p.Encode<int>(_npcTemplateID);
                EncodeData(p);
                return p;
            }
        }
    }
}