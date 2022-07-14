namespace Edelstein.Protocol.Network.Packets;

public interface IPacketWriter : IPacket
{
    long Length { get; }
    
    IPacketWriter WriteByte(byte value);
    IPacketWriter WriteBool(bool value);

    IPacketWriter WriteShort(short value);
    IPacketWriter WriteUShort(ushort value);

    IPacketWriter WriteInt(int value);
    IPacketWriter WriteUInt(uint value);

    IPacketWriter WriteLong(long value);
    IPacketWriter WriteULong(ulong value);

    IPacketWriter WriteDouble(double value);

    IPacketWriter WriteString(string value, short? length = null);
    IPacketWriter WriteBytes(byte[] value);
}
