namespace Edelstein.Protocol.Network.Transports;

public interface ITransportAcceptor : ITransport
{
    Task Accept(string host, int port);
}
