namespace Edelstein.Protocol.Network.Transports;

public interface ITransportConnector
{
    Task<ITransportContext> Connect(string host, int port);
}
