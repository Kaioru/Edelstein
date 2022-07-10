using Edelstein.Protocol.Network.Session;
using Edelstein.Protocol.Network.Transport;

namespace Tinystein
{
    public class TinysteinSessionInitializer : ISessionInitializer
    {
        public ISession Initialize(ISocket socket)
            => new TinysteinSession(socket);
    }
}
