using System.Text;
using Edelstein.Protocol.Util.Buffers.Bytes;

namespace Edelstein.Common.Util.Buffers.Bytes;

public class ByteWriter : IByteWriter
{
    private readonly Encoding _encoding = Encoding.ASCII;
    private readonly MemoryStream _stream;
    private readonly BinaryWriter _writer;

    public ByteWriter()
    {
        _stream = new MemoryStream();
        _writer = new BinaryWriter(_stream);
    }

    public ByteWriter(Enum operation) : this() => WriteShort(Convert.ToInt16(operation));

    public byte[] Buffer => _stream.ToArray();
    public long Length => _stream.Length;

    public IByteWriter WriteByte(byte value)
    {
        _writer.Write(value);
        return this;
    }

    public IByteWriter WriteBool(bool value)
    {
        _writer.Write(value);
        return this;
    }

    public IByteWriter WriteShort(short value)
    {
        _writer.Write(value);
        return this;
    }

    public IByteWriter WriteUShort(ushort value)
    {
        _writer.Write(value);
        return this;
    }

    public IByteWriter WriteInt(int value)
    {
        _writer.Write(value);
        return this;
    }

    public IByteWriter WriteUInt(uint value)
    {
        _writer.Write(value);
        return this;
    }

    public IByteWriter WriteLong(long value)
    {
        _writer.Write(value);
        return this;
    }

    public IByteWriter WriteULong(ulong value)
    {
        _writer.Write(value);
        return this;
    }

    public IByteWriter WriteDouble(double value)
    {
        _writer.Write(value);
        return this;
    }

    public IByteWriter WriteString(string value, short? length = null)
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

    public IByteWriter WriteBytes(byte[] value)
    {
        _writer.Write(value);
        return this;
    }
}
