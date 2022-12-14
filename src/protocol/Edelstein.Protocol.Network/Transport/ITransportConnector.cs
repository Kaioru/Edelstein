namespace Edelstein.Protocol.Network.Transport;

public interface ITransportConnector : ITransport
{
    ISocket? Socket { get; set; }

    Task Connect(string host, int port);
}
