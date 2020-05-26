using System;
using System.Drawing;
using System.IO;
using System.Text;

namespace Edelstein.Core.Network.Packets
{
    public class InPacket : IPacket, IPacketDecoder, IDisposable
    {
        private static readonly Encoding StringEncoding = Encoding.ASCII;

        private readonly MemoryStream _stream;
        private readonly BinaryReader _reader;

        public byte[] Buffer => _stream.ToArray();
        public int Length => (int) _stream.Length;

        public InPacket(byte[] buffer)
        {
            _stream = new MemoryStream(buffer);
            _reader = new BinaryReader(_stream);
        }

        public byte DecodeByte()
            => _reader.ReadByte();

        public bool DecodeBool()
            => DecodeByte() > 0;

        public short DecodeShort()
            => _reader.ReadInt16();

        public ushort DecodeUShort()
            => _reader.ReadUInt16();

        public int DecodeInt()
            => _reader.ReadInt32();

        public uint DecodeUInt()
            => _reader.ReadUInt32();

        public long DecodeLong()
            => _reader.ReadInt64();

        public string DecodeString(short? length = null)
            => StringEncoding.GetString(_reader.ReadBytes(length ?? DecodeShort()));

        public Point DecodePoint()
            => new Point(DecodeShort(), DecodeShort());

        public DateTime DecodeDateTime()
            => DateTime.FromFileTimeUtc(DecodeLong());

        public void Dispose()
        {
            //
        }
    }
}