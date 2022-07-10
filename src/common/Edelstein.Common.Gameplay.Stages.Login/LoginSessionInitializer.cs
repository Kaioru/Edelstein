using Edelstein.Common.Gameplay.Handling;
using Edelstein.Protocol.Network.Session;
using Edelstein.Protocol.Network.Transport;

namespace Edelstein.Common.Gameplay.Stages.Login
{
    public class LoginSessionInitializer : ISessionInitializer
    {
        private readonly IPacketProcessor<LoginStage, LoginStageUser> _processor;

        public LoginSessionInitializer(IPacketProcessor<LoginStage, LoginStageUser> processor)
            => _processor = processor;

        public ISession Initialize(ISocket socket) => new LoginStageUser(socket, _processor);
    }
}
