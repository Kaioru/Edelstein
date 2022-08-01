namespace Edelstein.Protocol.Util.Buffers.Bytes;

public interface IByteWriter : IByteBuffer
{
    long Length { get; }

    IByteWriter WriteByte(byte value);
    IByteWriter WriteBool(bool value);

    IByteWriter WriteShort(short value);
    IByteWriter WriteUShort(ushort value);

    IByteWriter WriteInt(int value);
    IByteWriter WriteUInt(uint value);

    IByteWriter WriteLong(long value);
    IByteWriter WriteULong(ulong value);

    IByteWriter WriteDouble(double value);

    IByteWriter WriteString(string value, short? length = null);
    IByteWriter WriteBytes(byte[] value);
}
