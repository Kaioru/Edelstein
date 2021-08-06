using System;
using System.Threading.Tasks;
using Edelstein.Common.Gameplay.Handling;
using Edelstein.Protocol.Network;

namespace Edelstein.Common.Gameplay.Stages.Login.Handlers
{
    public class LogoutWorldHandler : AbstractPacketHandler<LoginStage, LoginStageUser>
    {
        public override short Operation => (short)PacketRecvOperations.LogoutWorld;

        public override Task<bool> Check(LoginStageUser user)
            => Task.FromResult(user.State == LoginState.SelectCharacter);

        public override Task Handle(LoginStageUser user, IPacketReader packet)
        {
            user.State = LoginState.SelectWorld;
            user.AccountWorld = null;
            user.SelectedWorldID = null;
            user.SelectedChannelID = null;

            return Task.CompletedTask;
        }
    }
}
