using System.Threading.Tasks;
using Edelstein.Network.Packet;
using Edelstein.Service.Game.Fields.User;

namespace Edelstein.Service.Game.Interactions.Miniroom.Trade
{
    public class TradingRoom : AbstractMiniRoom
    {
        protected override MiniRoomType Type => MiniRoomType.TradingRoom;
        protected override byte MaxUsers => 2;

        public override Task OnPacket(MiniRoomAction action, FieldUser user, IPacket packet)
        {
            switch (action)
            {
                case MiniRoomAction.Leave:
                    return Close(MiniRoomLeaveType.Closed);
                default:
                    return base.OnPacket(action, user, packet);
            }
        }
    }
}