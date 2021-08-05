using System.Threading.Tasks;
using Edelstein.Common.Gameplay.Handling;
using Edelstein.Protocol.Network;

namespace Edelstein.Common.Gameplay.Stages.Login.Handlers
{
    public class WorldRequestHandler : AbstractPacketHandler<LoginStage, LoginStageUser>
    {
        public override short Operation => (short)PacketRecvOperations.WorldRequest;

        public override Task<bool> Check(LoginStageUser user)
            => Task.FromResult(
                user.Stage != null &&
                user.Account?.Gender != null &&
                user.AccountWorld == null
            );

        public override async Task Handle(LoginStageUser user, IPacketReader packet)
        {

        }
    }
}
