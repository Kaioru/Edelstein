using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
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

        protected readonly IDictionary<byte, IDictionary<byte, ItemSlot>> Item;
        protected readonly IDictionary<byte, int> Money;

        public TradingRoom()
        {
            Item = new Dictionary<byte, IDictionary<byte, ItemSlot>>();
            Money = new Dictionary<byte, int>();
        }

        public override async Task<bool> Enter(FieldUser user)
        {
            var result = await base.Enter(user);
            if (!result) return false;
            var pair = Users.FirstOrDefault(kv => kv.Value == user);
            Item[pair.Key] = new Dictionary<byte, ItemSlot>();
            Money[pair.Key] = 0;
            return true;
        }

        public override async Task Leave(FieldUser user, MiniRoomLeaveType type = MiniRoomLeaveType.Kicked)
        {
            var pair = Users.FirstOrDefault(kv => kv.Value == user);

            var items = Item[pair.Key].Values;
            var money = Money[pair.Key];

            await user.ModifyInventory(i => items.ForEach(i.Add));
            await user.ModifyStats(s => s.Money += money);

            Item.Remove(pair.Key);
            Money.Remove(pair.Key);
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
                case MiniRoomAction.MRP_Leave:
                    return Close(MiniRoomLeaveType.Closed);
                default:
                    return base.OnPacket(action, user, packet);
            }
        }

        private async Task OnPutItem(FieldUser user, IPacket packet)
        {
            if (Users.Count < 2) return;

            var pair = Users.FirstOrDefault(kv => kv.Value == user);
            var inventoryType = (ItemInventoryType) packet.Decode<byte>();
            var inventory = user.Character.GetInventory(inventoryType);
            var fromSlot = packet.Decode<short>();
            var number = packet.Decode<short>();
            var toSlot = packet.Decode<byte>();
            var item = inventory.Items.FirstOrDefault(i => i.Position == fromSlot);

            if (item == null) return;
            if (toSlot < 1 || toSlot > 9) return;
            if (Item[pair.Key].ContainsKey(toSlot)) return;

            IPacket GetPutItemPacket(byte position, byte slot, ItemSlot itemSlot)
            {
                using (var p = new Packet(SendPacketOperations.MiniRoom))
                {
                    p.Encode<byte>((byte) MiniRoomAction.TRP_PutItem);
                    p.Encode<byte>(position);
                    p.Encode<byte>(slot);
                    if (itemSlot is ItemSlotEquip equip) equip.Encode(p);
                    if (itemSlot is ItemSlotBundle bundle) bundle.Encode(p);
                    if (itemSlot is ItemSlotPet pet) pet.Encode(p);
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

                Item[pair.Key][toSlot] = item;
                await user.SendPacket(GetPutItemPacket(0x0, toSlot, item));
                await BroadcastPacket(user, GetPutItemPacket(0x1, toSlot, item));
            }, true);
        }

        private async Task OnPutMoney(FieldUser user, IPacket packet)
        {
            if (Users.Count < 2) return;

            var pair = Users.FirstOrDefault(kv => kv.Value == user);
            var amount = packet.Decode<int>();

            if (user.Character.Money < amount) return;

            Money[pair.Key] += amount;
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

            await user.SendPacket(GetPutMoneyPacket(0x0, Money[pair.Key]));
            await BroadcastPacket(user, GetPutMoneyPacket(0x1, Money[pair.Key]));
        }
    }
}