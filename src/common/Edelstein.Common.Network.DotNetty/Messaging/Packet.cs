using Edelstein.Protocol.Network.Messaging;

namespace Edelstein.Common.Network.DotNetty.Messaging;

public class Packet : IPacket
{
    public byte[] Buffer { get; }

    public Packet(byte[] buffer) => Buffer = buffer;
}
