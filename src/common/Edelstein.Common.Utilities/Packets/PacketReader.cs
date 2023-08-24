using System.Text;
using Edelstein.Protocol.Utilities.Packets;

namespace Edelstein.Common.Utilities.Packets;

public class PacketReader : IPacketReader
{
    private readonly Encoding _encoding = Encoding.ASCII;
    private readonly BinaryReader _reader;
    private readonly MemoryStream _stream;

    public PacketReader(byte[] buffer)
    {
        _stream = new MemoryStream(buffer);
        _reader = new BinaryReader(_stream);
    }

    public PacketReader(IPacket packet) : this(packet.Buffer)
    {
    }

    public PacketReader() : this(Array.Empty<byte>())
    {
    }

    public long Cursor => _stream.Position;
    public long Available => _stream.Length - _stream.Position;

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

    public IPacketReader Skip(short length)
    {
        ReadBytes(length);
        return this;
    }

    public void Dispose()
    {
        _reader.Dispose();
        _stream.Dispose();
    }
}
