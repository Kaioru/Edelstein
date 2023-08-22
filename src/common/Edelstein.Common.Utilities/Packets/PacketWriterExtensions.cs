using Edelstein.Protocol.Utilities.Packets;

namespace Edelstein.Common.Utilities.Packets;

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

    public static IPacketWriter WriteDateTime(
        this IPacketWriter writer,
        DateTime date
    )
    {
        writer.WriteLong(date.ToFileTimeUtc());
        return writer;
    }
}
