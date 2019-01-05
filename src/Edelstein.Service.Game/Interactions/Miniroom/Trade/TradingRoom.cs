using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
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

        private Task OnPutItem(FieldUser user, IPacket packet)
        {
            throw new System.NotImplementedException();
        }

        private async Task OnPutMoney(FieldUser user, IPacket packet)
        {
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