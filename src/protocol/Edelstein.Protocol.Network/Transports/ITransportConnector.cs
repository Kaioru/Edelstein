namespace Edelstein.Protocol.Network.Transports;

public interface ITransportConnector : ITransport
{
    IAdapter? Adapter { get; }

    Task Connect(string host, int port);
}
