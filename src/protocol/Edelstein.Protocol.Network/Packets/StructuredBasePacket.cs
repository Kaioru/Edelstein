using System.IO;

namespace Edelstein.Protocol.Network.Packets;

public record StructuredBasePacket : IDispatchable
{
    public void DispatchTo(Stream output)
    {
        using var writer = new RawPacketWriter();

        writer.WriteStructured(this);
        writer.DispatchTo(output);
    }
}
