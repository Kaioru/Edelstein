namespace Edelstein.Protocol.Util.Buffers.Packets;

public interface IPacketReadable
{
    void ReadFrom(IPacketReader reader);
}
