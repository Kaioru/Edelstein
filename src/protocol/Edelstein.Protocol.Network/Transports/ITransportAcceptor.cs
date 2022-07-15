namespace Edelstein.Protocol.Network.Transports;

public interface ITransportAcceptor : ITransport
{
    IDictionary<string, ISocket> Sockets { get; }

    Task Accept(string host, int port);
}
