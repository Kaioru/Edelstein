using Edelstein.Protocol.Util.Storages;

namespace Edelstein.Protocol.Network.Transport;

public interface ITransportAcceptor
{
    IStorage<string, ISocket> Sockets { get; }

    Task Accept(string host, int port);
}
