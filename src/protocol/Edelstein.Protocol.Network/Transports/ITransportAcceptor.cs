using Edelstein.Protocol.Util.Repositories;

namespace Edelstein.Protocol.Network.Transports;

public interface ITransportAcceptor : ITransport
{
    ILocalRepository<string, ISocket> Sockets { get; }

    Task Accept(string host, int port);
}
