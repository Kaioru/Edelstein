using System.Threading.Tasks;
using Edelstein.Common.Gameplay.Handling;
using Edelstein.Protocol.Network;

namespace Edelstein.Common.Gameplay.Stages.Login.Handlers
{
    public class SetGenderHandler : AbstractPacketHandler<LoginStage, LoginStageUser>
    {
        public override short Operation => (short)PacketRecvOperations.SetGender;

        public override Task<bool> Check(LoginStageUser user)
            => Task.FromResult(user.State == LoginState.SelectGender);

        public override async Task Handle(LoginStageUser user, IPacketReader packet)
        {
            var cancel = !packet.ReadBool();

            if (cancel)
            {
                await user.Disconnect();
                return;
            }

            var gender = (byte)(packet.ReadBool() ? 1 : 0);
            var response = new UnstructuredOutgoingPacket(PacketSendOperations.SetAccountResult);

            user.Account.Gender = gender;

            response.WriteByte(gender);
            response.WriteBool(true);

            await user.Dispatch(response);
        }
    }
}
