using System.Linq;
using System.Threading.Tasks;
using Edelstein.Core.Services;
using Edelstein.Network.Packet;
using Edelstein.Service.Game.Fields.User;

namespace Edelstein.Service.Game.Interactions.Miniroom.Trade
{
    public class TradingRoom : AbstractMiniRoom
    {
        protected override MiniRoomType Type => MiniRoomType.TradingRoom;
        protected override byte MaxUsers => 2;

        public override Task Leave(FieldUser user, MiniRoomLeaveType type = MiniRoomLeaveType.Kicked)
            => Close(type);

        public override async Task Close(MiniRoomLeaveType type = MiniRoomLeaveType.DestoryByAdmin)
        {
            await Task.WhenAll(Users.Select(async kv =>
            {
                using (var p = new Packet(SendPacketOperations.MiniRoom))
                {
                    p.Encode<byte>((byte) MiniRoomAction.Leave);
                    p.Encode<byte>(0x1);
                    p.Encode<byte>((byte) type);
                    await kv.Value.SendPacket(p);
                    await kv.Value.Interact(this, true);
                }
            }));
            Users.Clear();
        }

        public override async Task Chat(FieldUser user, string message)
        {
            using (var p = new Packet(SendPacketOperations.MiniRoom))
            {
                p.Encode<byte>((byte) MiniRoomAction.Chat);
                p.Encode<byte>((byte) MiniRoomAction.UserChat);
                p.Encode<byte>(0x1);
                p.Encode<string>(message);
                await user.SendPacket(p);
            }

            using (var p = new Packet(SendPacketOperations.MiniRoom))
            {
                p.Encode<byte>((byte) MiniRoomAction.Chat);
                p.Encode<byte>((byte) MiniRoomAction.UserChat);
                p.Encode<byte>(0x0);
                p.Encode<string>(message);
                await BroadcastPacket(user, p);
            }
        }

        public override Task OnPacket(MiniRoomAction action, FieldUser user, IPacket packet)
        {
            switch (action)
            {
                case MiniRoomAction.Leave:
                    return Leave(user, MiniRoomLeaveType.Closed);
                default:
                    return base.OnPacket(action, user, packet);
            }
        }
    }
}