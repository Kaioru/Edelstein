namespace Edelstein.Protocol.Network;

public interface INetworkAdapterInitializer
{
    INetworkAdapter Initialize(ISocket socket);
}
