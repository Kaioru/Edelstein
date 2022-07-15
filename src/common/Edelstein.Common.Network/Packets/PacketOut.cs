using System.Text;
using Edelstein.Protocol.Network.Packets;

namespace Edelstein.Common.Network.Packets;

public class PacketOut : IPacketWriter
{
    private readonly Encoding _encoding = Encoding.ASCII;
    private readonly MemoryStream _stream;
    private readonly BinaryWriter _writer;

    public PacketOut()
    {
        _stream = new MemoryStream();
        _writer = new BinaryWriter(_stream);
    }

    public PacketOut(Enum operation) : this() => WriteShort(Convert.ToInt16(operation));

    public byte[] Buffer => _stream.ToArray();
    public long Length => _stream.Length;

    public IPacketWriter WriteByte(byte value)
    {
        _writer.Write(value);
        return this;
    }

    public IPacketWriter WriteBool(bool value)
    {
        _writer.Write(value);
        return this;
    }

    public IPacketWriter WriteShort(short value)
    {
        _writer.Write(value);
        return this;
    }

    public IPacketWriter WriteUShort(ushort value)
    {
        _writer.Write(value);
        return this;
    }

    public IPacketWriter WriteInt(int value)
    {
        _writer.Write(value);
        return this;
    }

    public IPacketWriter WriteUInt(uint value)
    {
        _writer.Write(value);
        return this;
    }

    public IPacketWriter WriteLong(long value)
    {
        _writer.Write(value);
        return this;
    }

    public IPacketWriter WriteULong(ulong value)
    {
        _writer.Write(value);
        return this;
    }

    public IPacketWriter WriteDouble(double value)
    {
        _writer.Write(value);
        return this;
    }

    public IPacketWriter WriteString(string value, short? length = null)
    {
        value ??= string.Empty;

        if (length.HasValue)
        {
            if (value.Length > length) value = value.Substring(0, length.Value);
            WriteBytes(_encoding.GetBytes(value.PadRight(length.Value, '\0')));
        }
        else
        {
            WriteShort((short)_encoding.GetByteCount(value));
            WriteBytes(_encoding.GetBytes(value));
        }

        return this;
    }

    public IPacketWriter WriteBytes(byte[] value)
    {
        _writer.Write(value);
        return this;
    }
}
