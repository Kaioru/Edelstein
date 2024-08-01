using System.IO;
using System.Threading.Tasks;

namespace Edelstein.Protocol.Network.Packets;

public record StructuredBasePacket : IDispatchable
{
    public void DispatchTo(Stream output) 
        => StructuredPacketSerializer.Shared.Serialize(output, this);
}
