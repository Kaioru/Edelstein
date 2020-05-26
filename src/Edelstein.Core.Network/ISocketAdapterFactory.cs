namespace Edelstein.Core.Network
{
    public interface ISocketAdapterFactory
    {
        ISocketAdapter Build(ISocket socket);
    }
}