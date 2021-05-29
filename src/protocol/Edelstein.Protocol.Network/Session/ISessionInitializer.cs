using Edelstein.Protocol.Network.Transport;

namespace Edelstein.Protocol.Network.Session
{
    public interface ISessionInitializer
    {
        ISession Initialize(ISocket socket);
    }
}
