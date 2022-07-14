namespace Edelstein.Protocol.Network;

public interface IAdapterInitializer
{
    IAdapter Initialize(ISocket socket);
}
