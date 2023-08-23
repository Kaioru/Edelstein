using Edelstein.Protocol.Utilities.Packets;

namespace Edelstein.Common.Utilities.Packets;

public class Packet : IPacket
{

    public Packet(byte[] buffer) => Buffer = buffer;
    public byte[] Buffer { get; }
}
