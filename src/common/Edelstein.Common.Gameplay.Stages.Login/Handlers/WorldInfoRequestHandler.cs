using Edelstein.Common.Gameplay.Handling;

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
    }
}
