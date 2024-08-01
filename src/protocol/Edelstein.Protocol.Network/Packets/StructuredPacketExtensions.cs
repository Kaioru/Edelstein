namespace Edelstein.Protocol.Network.Packets;

public static class StructuredPacketExtensions
{
    public static IRawPacket ToRawPacket(this StructuredBasePacket packet)
    {
        using var stream = RawPacketMemory.Shared.GetStream();
        StructuredPacketSerializer.Shared.Serialize(stream, packet);
        return new RawPacket(stream);
    }
}
