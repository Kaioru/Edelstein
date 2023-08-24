namespace Edelstein.Protocol.Network.Transports;

public interface ITransportAcceptor
{
    Task<ITransportContext> Accept(string host, int port);
}
