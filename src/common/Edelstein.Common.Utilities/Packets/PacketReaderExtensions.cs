using Edelstein.Protocol.Utilities.Packets;

namespace Edelstein.Common.Utilities.Packets;

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

    public static DateTime ReadDateTime(
        this IPacketReader reader
    ) => DateTime.FromFileTimeUtc(reader.ReadLong());
}
