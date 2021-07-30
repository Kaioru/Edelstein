using Edelstein.Protocol.Network.Session;
using Edelstein.Protocol.Network.Transport;

namespace Edelstein.Common.Gameplay.Stages.Game
{
    public class GameSessionInitializer : ISessionInitializer
    {
        public ISession Initialize(ISocket socket) => new GameStageUser(socket);
    }
}
