namespace Edelstein.Protocol.Network.Transport;

public interface ITransportConnector : ITransport
{
    ISocket? Socket { get; }

    Task Connect(string host, int port);
}
