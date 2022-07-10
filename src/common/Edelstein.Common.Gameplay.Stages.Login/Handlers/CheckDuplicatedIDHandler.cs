using System.Threading.Tasks;
using Edelstein.Common.Gameplay.Handling;
using Edelstein.Protocol.Network;

namespace Edelstein.Common.Gameplay.Stages.Login.Handlers
{
    public class CheckDuplicatedIDHandler : AbstractPacketHandler<LoginStage, LoginStageUser>
    {
        public override short Operation => (short)PacketRecvOperations.CheckDuplicatedID;

        public override Task<bool> Check(LoginStageUser user)
            => Task.FromResult(user.State == LoginState.SelectCharacter);

        public override async Task Handle(LoginStageUser user, IPacketReader packet)
        {
            var name = packet.ReadString();

            var result = await user.Stage.CharacterRepository.CheckExistsByName(name);
            var response = new UnstructuredOutgoingPacket(PacketSendOperations.CheckDuplicatedIDResult);

            response.WriteString(name);
            response.WriteBool(result);

            await user.Dispatch(response);
        }
    }
}
