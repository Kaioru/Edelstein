using System.Threading.Tasks;
using Edelstein.Common.Gameplay.Handling;
using Edelstein.Protocol.Network;
using Microsoft.Extensions.Logging;

namespace Edelstein.Common.Gameplay.Stages.Login.Handlers
{
    public class ExceptionLogHandler : AbstractPacketHandler<LoginStage, LoginStageUser>
    {
        public override short Operation => (short)PacketRecvOperations.ExceptionLog;
        private readonly LoginStage _stage;

        public ExceptionLogHandler(LoginStage stage)
            => _stage = stage;

        public override Task<bool> Check(LoginStageUser user)
            => Task.FromResult(user.State == LoginState.LoggedOut);

        public override Task Handle(LoginStageUser user, IPacketReader packet)
        {
            var log = packet.ReadString();

            _stage.Logger.LogWarning($"Received exception log from client: {log}");
            return Task.CompletedTask;
        }
    }
}
