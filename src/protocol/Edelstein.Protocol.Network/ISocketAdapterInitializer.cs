namespace Edelstein.Protocol.Network;

public interface ISocketAdapterInitializer
{
    ISocketAdapter Initialize(ISocket socket);
}
