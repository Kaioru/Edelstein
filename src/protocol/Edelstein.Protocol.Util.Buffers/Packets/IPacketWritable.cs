namespace Edelstein.Protocol.Util.Buffers.Packets;

public interface IPacketWritable
{
    void WriteTo(IPacketWriter writer);
}
