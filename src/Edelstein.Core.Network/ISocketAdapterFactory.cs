namespace Edelstein.Network
{
    public interface ISocketAdapterFactory
    {
        ISocketAdapter Build(ISocket socket);
    }
}