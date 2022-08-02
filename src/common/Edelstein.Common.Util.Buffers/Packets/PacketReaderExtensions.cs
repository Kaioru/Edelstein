using Edelstein.Protocol.Util.Buffers.Bytes;

namespace Edelstein.Common.Util.Buffers.Packets;

public static class PacketReaderExtensions
{
    public static IPacketReadable Read(
        this IPacketReader reader,
        IPacketReadable readable
    )
    {
        readable.ReadFrom(reader);
        return readable;
    }
}
