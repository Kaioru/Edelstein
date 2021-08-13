using System.Threading.Tasks;
using Edelstein.Common.Gameplay.Handling;
using Edelstein.Common.Gameplay.Stages.Game.Movements;
using Edelstein.Protocol.Network;

namespace Edelstein.Common.Gameplay.Stages.Game.Handlers
{
    public class UserMoveHandler : AbstractPacketHandler<GameStage, GameStageUser>
    {
        public override short Operation => (short)PacketRecvOperations.UserMove;

        public override Task<bool> Check(GameStageUser user)
            => Task.FromResult(user.Field != null && user.FieldUser != null);

        public override async Task Handle(GameStageUser user, IPacketReader packet)
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

            await user.FieldUser.Move(path);
            await user.FieldUser.FieldSplit.Dispatch(user.FieldUser, response);
        }
    }
}
