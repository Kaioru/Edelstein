using System.Threading.Tasks;
using Edelstein.Common.Gameplay.Handling;
using Edelstein.Common.Gameplay.Stages.Login.Types;
using Edelstein.Protocol.Network;

namespace Edelstein.Common.Gameplay.Stages.Login.Handlers
{
    public class CheckPinCodeHandler : AbstractPacketHandler<LoginStage, LoginStageUser>
    {
        public override short Operation => (short)PacketRecvOperations.CheckPinCode;

        public override Task<bool> Check(LoginStageUser user)
            => Task.FromResult(user.State == LoginState.SelectGender);

        public override async Task Handle(LoginStageUser user, IPacketReader packet)
        {
            var response = new UnstructuredOutgoingPacket(PacketSendOperations.CheckPinCodeResult);

            response.WriteByte((byte)LoginResultCode.Success);

            user.State = LoginState.SelectWorld;

            await user.Dispatch(response);
        }
    }
}
