using System;
using System.IO;
using System.Text;
using Edelstein.Protocol.Util.Spatial;

namespace Edelstein.Protocol.Network
{
    public class UnstructuredIncomingPacket : IPacketReader
    {
        private static readonly Encoding StringEncoding = Encoding.ASCII;

        private readonly MemoryStream _stream;
        private readonly BinaryReader _reader;

        public ReadOnlySpan<byte> Buffer => _stream.ToArray();
        public long Cursor => _stream.Position;
        public long Available => _stream.Length - _stream.Position;

        public UnstructuredIncomingPacket(byte[] buffer)
        {
            _stream = new MemoryStream(buffer);
            _reader = new BinaryReader(_stream);
        }

        public UnstructuredIncomingPacket() : this(Array.Empty<byte>()) { }

        public byte ReadByte() => _reader.ReadByte();
        public bool ReadBool() => _reader.ReadBoolean();

        public short ReadShort() => _reader.ReadInt16();
        public ushort ReadUShort() => _reader.ReadUInt16();

        public int ReadInt() => _reader.ReadInt32();
        public uint ReadUInt() => _reader.ReadUInt32();

        public long ReadLong() => _reader.ReadInt64();
        public ulong ReadULong() => _reader.ReadUInt64();

        public string ReadString(short? length = null) => StringEncoding.GetString(_reader.ReadBytes(length ?? ReadShort()));
        public byte[] ReadBytes(short length) => _reader.ReadBytes(length);

        public Point2D ReadPoint2D() => new(ReadShort(), ReadShort());

        public DateTime ReadDateTime() => DateTime.FromFileTimeUtc(ReadLong());
    }
}