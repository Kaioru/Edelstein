namespace Edelstein.Protocol.Network.Transports;

public interface ITransportConnector : ITransport
{
    ISocket? Socket { get; }

    Task Connect(string host, int port);
}
