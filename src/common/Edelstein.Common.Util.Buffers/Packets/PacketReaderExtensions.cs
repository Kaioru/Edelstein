using Edelstein.Common.Util.Spatial;
using Edelstein.Protocol.Util.Buffers.Packets;
using Edelstein.Protocol.Util.Spatial;

namespace Edelstein.Common.Util.Buffers.Packets;

public static class PacketReaderExtensions
{
    public static T Read<T>(
        this IPacketReader reader,
        T readable
    ) where T : IPacketReadable
    {
        readable.ReadFrom(reader);
        return readable;
    }

    public static IPoint2D ReadPoint2D(
        this IPacketReader reader
    ) =>
        new Point2D(
            reader.ReadShort(),
            reader.ReadShort()
        );

    public static DateTime ReadDateTime(
        this IPacketReader reader
    ) => DateTime.FromFileTimeUtc(reader.ReadLong());
}
