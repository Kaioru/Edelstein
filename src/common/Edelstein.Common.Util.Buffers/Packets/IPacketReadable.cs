using Edelstein.Protocol.Util.Buffers.Bytes;

namespace Edelstein.Common.Util.Buffers.Packets;

public interface IPacketReadable
{
    void ReadFrom(IPacketReader reader);
}
