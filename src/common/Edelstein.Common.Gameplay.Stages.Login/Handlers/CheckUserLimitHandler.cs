using System.Threading.Tasks;
using Edelstein.Common.Gameplay.Handling;
using Edelstein.Protocol.Network;

namespace Edelstein.Common.Gameplay.Stages.Login.Handlers
{
    public class CheckUserLimitHandler : AbstractPacketHandler<LoginStage, LoginStageUser>
    {
        public override short Operation => (short)PacketRecvOperations.CheckUserLimit;

        public override Task<bool> Check(LoginStageUser user)
            => Task.FromResult(user.State == LoginState.SelectWorld);

        public override async Task Handle(LoginStageUser user, IPacketReader packet)
        {
            _ = packet.ReadByte(); // WorldID

            var response = new UnstructuredOutgoingPacket(PacketSendOperations.CheckUserLimitResult);

            response.WriteByte(0); // TODO: bWarningLevel
            response.WriteByte(0); // TODO: bPopulateLevel

            await user.Dispatch(response);
        }
    }
}
