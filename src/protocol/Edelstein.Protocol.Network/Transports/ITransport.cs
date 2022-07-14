using Edelstein.Protocol.Network.Packets;

namespace Edelstein.Protocol.Network.Transports;

public interface ITransport
{
    ISocketAdapterInitializer Initializer { get; }
    
    short Version { get; }
    string Patch { get; }
    byte Locale { get; }

    Task Dispatch(IPacket packet);
    Task Close();
}
