namespace Edelstein.Protocol.Network.Transports;

public interface ITransportConnector : ITransport
{
    Task Connect(string host, int port);
}
