namespace Edelstein.Protocol.Utilities.Packets;

public interface IPacketReadable
{
    void ReadFrom(IPacketReader reader);
}
