using System;
using System.IO;
using System.Text;
using Microsoft.IO;

namespace Edelstein.Protocol.Network.Packets;

public class RawPacketWriter : IRawPacketWriter
{
    private readonly Encoding _encoding = Encoding.ASCII;
    private readonly RecyclableMemoryStream _stream;
    private readonly BinaryWriter _writer;

    public long Length => _stream.Length;

    public RawPacketWriter()
    {
        _stream = RawPacketMemory.Shared.GetStream();
        _writer = new BinaryWriter(_stream);
    }

    public RawPacketWriter(IFormattable operation) : this() 
        => WriteShort(Convert.ToInt16(operation));

    public IRawPacketWriter WriteByte(byte value)
    {
        _writer.Write(value);
        return this;
    }

    public IRawPacketWriter WriteBool(bool value)
    {
        _writer.Write(value);
        return this;
    }

    public IRawPacketWriter WriteShort(short value)
    {
        _writer.Write(value);
        return this;
    }

    public IRawPacketWriter WriteUShort(ushort value)
    {
        _writer.Write(value);
        return this;
    }

    public IRawPacketWriter WriteInt(int value)
    {
        _writer.Write(value);
        return this;
    }

    public IRawPacketWriter WriteUInt(uint value)
    {
        _writer.Write(value);
        return this;
    }

    public IRawPacketWriter WriteLong(long value)
    {
        _writer.Write(value);
        return this;
    }

    public IRawPacketWriter WriteULong(ulong value)
    {
        _writer.Write(value);
        return this;
    }

    public IRawPacketWriter WriteDouble(double value)
    {
        _writer.Write(value);
        return this;
    }

    public IRawPacketWriter WriteString(string value, short? length = null)
    {
        if (length.HasValue)
        {
            if (value.Length > length) value = value[..length.Value];
            WriteBytes(_encoding.GetBytes(value.PadRight(length.Value, '\0')));
        }
        else
        {
            WriteShort((short)_encoding.GetByteCount(value));
            WriteBytes(_encoding.GetBytes(value));
        }

        return this;
    }

    public IRawPacketWriter WriteBytes(byte[] value)
    {
        _writer.Write(value);
        return this;
    }

    public IRawPacketWriter WriteStructured<T>(T obj) where T : StructuredBasePacket
    {
        StructuredPacketSerializer.Shared.Serialize(_stream, obj);
        return this;
    }

    public void DispatchTo(Stream output)
    {
        foreach (var memory in _stream.GetReadOnlySequence())
            output.Write(memory.Span);
    }
    
    public void Dispose()
    {
        GC.SuppressFinalize(this);
        
        _writer.Dispose();
        _stream.Dispose();
    }
}
