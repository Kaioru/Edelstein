using Edelstein.Protocol.Util.Buffers.Bytes;

namespace Edelstein.Protocol.Network.Transports;

public interface ITransport
{
    IAdapterInitializer Initializer { get; }

    short Version { get; }
    string Patch { get; }
    byte Locale { get; }

    Task Dispatch(IPacket packet);
    Task Close();
}
