using Edelstein.Protocol.Util.Buffers.Bytes;

namespace Edelstein.Common.Util.Buffers.Packets;

public interface IPacketWritable
{
    void WriteTo(IPacketWriter writer);
}
