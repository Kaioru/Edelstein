using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Edelstein.Core.Constants;
using Edelstein.Core.Extensions;
using Edelstein.Core.Services;
using Edelstein.Data.Entities.Inventory;
using Edelstein.Network.Packet;
using Edelstein.Service.Game.Fields.User;
using MoreLinq;

namespace Edelstein.Service.Game.Interactions.Miniroom.Trade
{
    public class TradingRoom : AbstractMiniRoom
    {
        protected override MiniRoomType Type => MiniRoomType.TradingRoom;
        protected override byte MaxUsers => 2;

        private bool _locked => _locks.Count > 0;
        private readonly ICollection<byte> _locks;
        private readonly IDictionary<byte, IDictionary<byte, ItemSlot>> _item;
        private readonly IDictionary<byte, int> _money;

        public TradingRoom()
        {
            _locks = new HashSet<byte>();
            _item = new Dictionary<byte, IDictionary<byte, ItemSlot>>();
            _money = new Dictionary<byte, int>();
        }

        public override async Task<bool> Enter(FieldUser user)
        {
            var result = await base.Enter(user);
            if (!result) return false;
            var pair = Users.FirstOrDefault(kv => kv.Value == user);
            _item[pair.Key] = new Dictionary<byte, ItemSlot>();
            _money[pair.Key] = 0;
            return true;
        }

        public override async Task Leave(FieldUser user, MiniRoomLeaveType type = MiniRoomLeaveType.Kicked)
        {
            var pair = Users.FirstOrDefault(kv => kv.Value == user);

            var items = _item[pair.Key].Values;
            var money = _money[pair.Key];

            await user.ModifyInventory(i => items.ForEach(i.Add));
            await user.ModifyStats(s => s.Money += money);

            _item.Remove(pair.Key);
            _money.Remove(pair.Key);
            await base.Leave(user, type);
        }

        public override Task OnPacket(MiniRoomAction action, FieldUser user, IPacket packet)
        {
            switch (action)
            {
                case MiniRoomAction.TRP_PutItem:
                    return OnPutItem(user, packet);
                case MiniRoomAction.TRP_PutMoney:
                    return OnPutMoney(user, packet);
                case MiniRoomAction.TRP_Trade:
                    return OnTrade(user, packet);
                case MiniRoomAction.MRP_Leave:
                    return Close();
                default:
                    return base.OnPacket(action, user, packet);
            }
        }

        private async Task OnPutItem(FieldUser user, IPacket packet)
        {
            if (Users.Count < 2) return;
            if (_locked) return;

            var pair = Users.FirstOrDefault(kv => kv.Value == user);
            var inventoryType = (ItemInventoryType) packet.Decode<byte>();
            var inventory = user.Character.GetInventory(inventoryType);
            var fromSlot = packet.Decode<short>();
            var number = packet.Decode<short>();
            var toSlot = packet.Decode<byte>();
            var item = inventory.Items.FirstOrDefault(i => i.Position == fromSlot);

            if (item == null) return;
            if (toSlot < 1 || toSlot > 9) return;
            if (_item[pair.Key].ContainsKey(toSlot)) return;

            IPacket GetPutItemPacket(byte position, byte slot, ItemSlot itemSlot)
            {
                using (var p = new Packet(SendPacketOperations.MiniRoom))
                {
                    p.Encode<byte>((byte) MiniRoomAction.TRP_PutItem);
                    p.Encode<byte>(position);
                    p.Encode<byte>(slot);
                    itemSlot.Encode(p);
                    return p;
                }
            }

            await user.ModifyInventory(async i =>
            {
                if (!ItemConstants.IsTreatSingly(item.TemplateID))
                {
                    if (!(item is ItemSlotBundle bundle)) return;
                    if (bundle.Number < number) return;

                    item = i.Take(bundle, number);
                }
                else i.Remove(item);

                _item[pair.Key][toSlot] = item;
                await user.SendPacket(GetPutItemPacket(0x0, toSlot, item));
                await BroadcastPacket(user, GetPutItemPacket(0x1, toSlot, item));
            }, true);
        }

        private async Task OnPutMoney(FieldUser user, IPacket packet)
        {
            if (Users.Count < 2) return;
            if (_locked) return;

            var pair = Users.FirstOrDefault(kv => kv.Value == user);
            var amount = packet.Decode<int>();

            if (user.Character.Money < amount) return;

            _money[pair.Key] += amount;
            await user.ModifyStats(s => s.Money -= amount, true);

            IPacket GetPutMoneyPacket(byte position, int money)
            {
                using (var p = new Packet(SendPacketOperations.MiniRoom))
                {
                    p.Encode<byte>((byte) MiniRoomAction.TRP_PutMoney);
                    p.Encode<byte>(position);
                    p.Encode<int>(money);
                    return p;
                }
            }

            await user.SendPacket(GetPutMoneyPacket(0x0, _money[pair.Key]));
            await BroadcastPacket(user, GetPutMoneyPacket(0x1, _money[pair.Key]));
        }

        private async Task OnTrade(FieldUser user, IPacket packet)
        {
            if (Users.Count < 2) return;

            var pair = Users.FirstOrDefault(kv => kv.Value == user);

            if (_locks.Contains(pair.Key)) return;
            _locks.Add(pair.Key);

            using (var p = new Packet(SendPacketOperations.MiniRoom))
            {
                p.Encode<byte>((byte) MiniRoomAction.TRP_Trade);
                await BroadcastPacket(user, p);
            }

            if (_locks.Count == MaxUsers)
            {
                var self = Users[0];
                var target = Users[1];

                if (self.Character.HasSlotFor(_item[1].Values) &&
                    target.Character.HasSlotFor(_item[0].Values) &&
                    int.MaxValue - self.Character.Money >= _money[0] &&
                    int.MaxValue - target.Character.Money >= _money[1])
                {
                    var selfItem = _item[0];
                    var selfMoney = _money[0];

                    _item[0] = _item[1];
                    _money[0] = _money[1];
                    _item[1] = selfItem;
                    _money[1] = selfMoney;
                    await Close(MiniRoomLeaveType.TradeDone);
                }
                else await Close(MiniRoomLeaveType.TradeFail);
            }
        }
    }
}