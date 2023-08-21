using Edelstein.Protocol.Utilities.Packets;

namespace Edelstein.Common.Utilities.Packets;

public class Packet : IPacket
{
    public byte[] Buffer { get; }
    
    public Packet(byte[] buffer) => Buffer = buffer;
}
