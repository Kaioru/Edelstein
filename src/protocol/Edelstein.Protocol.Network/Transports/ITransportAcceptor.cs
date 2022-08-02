namespace Edelstein.Protocol.Network.Transports;

public interface ITransportAcceptor : ITransport
{
    IDictionary<string, ISocket> Sockets { get; }

    TimeSpan Timeout { get; }

    Task Accept(string host, int port);
}
