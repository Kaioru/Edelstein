using Edelstein.Protocol.Network.Session;

namespace Edelstein.Protocol.Network.Transport
{
    public interface ITransport : IPacketDispatcher
    {
        ISessionInitializer SessionInitializer { get; init; }

        short Version { get; init; }
        string Patch { get; init; }
        byte Locale { get; init; }
    }
}
