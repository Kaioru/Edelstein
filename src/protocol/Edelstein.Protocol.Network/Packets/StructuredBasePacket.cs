using System.IO;

namespace Edelstein.Protocol.Network.Packets;

public record StructuredBasePacket : IDispatchable
{
    public void DispatchTo(Stream output) 
        => StructuredPacketSerializer.Shared.Serialize(output, this);
}
