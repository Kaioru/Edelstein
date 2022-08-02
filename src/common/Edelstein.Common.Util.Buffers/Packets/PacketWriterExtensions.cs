using Edelstein.Protocol.Util.Buffers.Bytes;

namespace Edelstein.Common.Util.Buffers.Packets;

public static class PacketWriterExtensions
{
    public static IPacketWriter Write(
        this IPacketWriter writer,
        IPacketWritable writable
    )
    {
        writable.WriteTo(writer);
        return writer;
    }
}
