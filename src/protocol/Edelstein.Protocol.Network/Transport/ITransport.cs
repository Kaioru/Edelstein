using Edelstein.Protocol.Network.Messaging;

namespace Edelstein.Protocol.Network.Transport;

public interface ITransport
{
    TransportState State { get; }

    short Version { get; }
    string Patch { get; }
    byte Locale { get; }

    Task Dispatch(IPacket packet);
    Task Close();
}
