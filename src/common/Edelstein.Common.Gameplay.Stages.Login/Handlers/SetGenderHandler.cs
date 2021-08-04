using System;
using System.Threading.Tasks;
using Edelstein.Common.Gameplay.Handling;
using Edelstein.Protocol.Network;

namespace Edelstein.Common.Gameplay.Stages.Login.Handlers
{
    public class SetGenderHandler : AbstractPacketHandler<LoginStage, LoginStageUser>
    {
        public override short Operation => (short)PacketRecvOperations.SetGender;

        public override Task<bool> Check(LoginStageUser user)
            => Task.FromResult(user.Stage != null && user.Account != null);

        public override Task Handle(LoginStageUser user, IPacketReader packet)
        {
            // TODO
            throw new NotImplementedException();
        }
    }
}
