using Edelstein.Common.Gameplay.Handling;
using Edelstein.Protocol.Network.Session;
using Edelstein.Protocol.Network.Transport;

namespace Edelstein.Common.Gameplay.Stages.Game
{
    public class GameSessionInitializer : ISessionInitializer
    {
        private readonly IPacketProcessor<GameStage, GameStageUser> _processor;

        public GameSessionInitializer(IPacketProcessor<GameStage, GameStageUser> processor)
            => _processor = processor;

        public ISession Initialize(ISocket socket) => new GameStageUser(socket, _processor);
    }
}
