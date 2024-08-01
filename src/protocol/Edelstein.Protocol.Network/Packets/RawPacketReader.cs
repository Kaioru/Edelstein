using System;
using System.IO;
using System.Text;
using CommunityToolkit.HighPerformance;

namespace Edelstein.Protocol.Network.Packets;

public class RawPacketReader : IRawPacketReader
{
    private readonly Encoding _encoding = Encoding.ASCII;
    private readonly IRawPacket _packet;
    private readonly Stream _stream;
    private readonly BinaryReader _reader;
    
    public long Cursor => _stream.Position;
    public long Available => _stream.Length - _stream.Position;

    public RawPacketReader(IRawPacket packet)
    {
        _packet = packet;
        _stream = packet.Buffer.AsStream();
        _reader = new BinaryReader(_stream);
    }

    public byte ReadByte() => _reader.ReadByte();
    public bool ReadBool() => _reader.ReadBoolean();

    public short ReadShort() => _reader.ReadInt16();
    public ushort ReadUShort() => _reader.ReadUInt16();

    public int ReadInt() => _reader.ReadInt32();
    public uint ReadUInt() => _reader.ReadUInt32();

    public long ReadLong() => _reader.ReadInt64();
    public ulong ReadULong() => _reader.ReadUInt64();

    public double ReadDouble() => _reader.ReadDouble();

    public string ReadString(short? length = null) => _encoding.GetString(_reader.ReadBytes(length ?? ReadShort()));

    public byte[] ReadBytes(short length) => _reader.ReadBytes(length);

    public IRawPacketReader Skip(short length)
    {
        ReadBytes(length);
        return this;
    }

    public void DispatchTo(Stream output) 
        => output.Write(_packet.Buffer.Span);

    public void Dispose()
    {
        GC.SuppressFinalize(this);
        
        _reader.Dispose();
        _stream.Dispose();
    }
}
