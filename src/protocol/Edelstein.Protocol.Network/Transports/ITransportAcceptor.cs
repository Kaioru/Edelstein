using Edelstein.Protocol.Util.Repositories;

namespace Edelstein.Protocol.Network.Transports;

public interface ITransportAcceptor : ITransport
{
    ILocalRepository<string, IAdapter> Adapters { get; }

    Task Accept(string host, int port);
}
