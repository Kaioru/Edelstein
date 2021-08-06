using System.Threading.Tasks;
using Edelstein.Common.Gameplay.Handling;
using Edelstein.Protocol.Network;

namespace Edelstein.Common.Gameplay.Stages.Login.Handlers
{
    public class WorldInfoRequestHandler : MirroredPacketHandler<LoginStage, LoginStageUser>
    {
        public WorldInfoRequestHandler() : base(
            (short)PacketRecvOperations.WorldInfoRequest,
            new WorldRequestHandler()
        )
        {
        }

        public override Task<bool> Check(LoginStageUser user)
            => Task.FromResult(user.State == LoginState.SelectCharacter);

        public override Task Handle(LoginStageUser user, IPacketReader packet)
        {
            user.State = LoginState.SelectWorld;
            user.SelectedWorldID = null;
            user.SelectedChannelID = null;

            return base.Handle(user, packet);
        }
    }
}
