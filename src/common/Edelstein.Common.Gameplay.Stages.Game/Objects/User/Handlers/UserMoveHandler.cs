using System.Threading.Tasks;
using Edelstein.Common.Gameplay.Handling;
using Edelstein.Common.Gameplay.Stages.Game.Movements;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects;
using Edelstein.Protocol.Network;

namespace Edelstein.Common.Gameplay.Stages.Game.Objects.User.Handlers
{
    public class UserMoveHandler : AbstractUserPacketHandler
    {
        public override short Operation => (short)PacketRecvOperations.UserMove;

        protected override async Task Handle(IFieldObjUser user, IPacketReader packet)
        {
            _ = packet.ReadLong();
            _ = packet.ReadByte();
            _ = packet.ReadLong();
            _ = packet.ReadInt();
            _ = packet.ReadInt();
            _ = packet.ReadInt();

            var path = packet.Read(new MovePath());
            var response = new UnstructuredOutgoingPacket(PacketSendOperations.UserMove);

            response.WriteInt(user.ID);
            response.Write(path);

            await user.Move(path);
            await user.FieldSplit.Dispatch(user, response);
        }
    }
}
