using Edelstein.Protocol.Network.Session;
using Edelstein.Protocol.Network.Transport;

namespace Edelstein.Common.Gameplay.Stages.Login
{
    public class LoginSessionInitializer : ISessionInitializer
    {
        public ISession Initialize(ISocket socket) => new LoginStageUser(socket);
    }
}
