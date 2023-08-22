namespace Edelstein.Protocol.Utilities.Packets;

public interface IPacketWritable
{
    void WriteTo(IPacketWriter writer);
}
