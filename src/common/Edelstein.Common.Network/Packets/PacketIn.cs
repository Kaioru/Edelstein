using System.Text;
using Edelstein.Protocol.Network.Packets;

namespace Edelstein.Common.Network.Packets;

public class PacketIn : IPacketReader
{
    private readonly Encoding _encoding = Encoding.ASCII;
    private readonly BinaryReader _reader;
    private readonly MemoryStream _stream;

    public PacketIn(byte[] buffer)
    {
        _stream = new MemoryStream(buffer);
        _reader = new BinaryReader(_stream);
    }

    public PacketIn() : this(Array.Empty<byte>())
    {
    }

    public byte[] Buffer => _stream.ToArray();
    public long Cursor => _stream.Position;
    public long Available => _stream.Length - _stream.Position;

    public byte ReadByte()
    {
        return _reader.ReadByte();
    }

    public bool ReadBool()
    {
        return _reader.ReadBoolean();
    }

    public short ReadShort()
    {
        return _reader.ReadInt16();
    }

    public ushort ReadUShort()
    {
        return _reader.ReadUInt16();
    }

    public int ReadInt()
    {
        return _reader.ReadInt32();
    }

    public uint ReadUInt()
    {
        return _reader.ReadUInt32();
    }

    public long ReadLong()
    {
        return _reader.ReadInt64();
    }

    public ulong ReadULong()
    {
        return _reader.ReadUInt64();
    }

    public double ReadDouble()
    {
        return _reader.ReadDouble();
    }

    public string ReadString(short? length = null)
    {
        return _encoding.GetString(_reader.ReadBytes(length ?? ReadShort()));
    }

    public byte[] ReadBytes(short length)
    {
        return _reader.ReadBytes(length);
    }
}
