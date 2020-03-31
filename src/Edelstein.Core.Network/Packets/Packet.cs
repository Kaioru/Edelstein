using System;
using System.Drawing;
using System.Text;
using DotNetty.Buffers;

namespace Edelstein.Network.Packets
{
    public class Packet : IPacket
    {
        private static readonly Encoding StringEncoding = Encoding.ASCII;

        private readonly IByteBuffer _buffer;

        public byte[] Buffer => _buffer.Array;
        public int Length => _buffer.ReadableBytes;

        public Packet(IByteBuffer buffer)
            => _buffer = buffer;

        public Packet(Enum operation, int initialCapacity = byte.MaxValue)
            : this(Unpooled.Buffer(initialCapacity))
            => EncodeShort(Convert.ToInt16(operation));

        public Packet(int initialCapacity = byte.MaxValue)
            : this(Unpooled.Buffer(initialCapacity))
        {
        }

        public byte DecodeByte()
            => _buffer.ReadByte();

        public bool DecodeBool()
            => _buffer.ReadByte() > 0;

        public short DecodeShort()
            => _buffer.ReadShortLE();

        public ushort DecodeUShort()
            => _buffer.ReadUnsignedShortLE();

        public int DecodeInt()
            => _buffer.ReadIntLE();

        public uint DecodeUInt()
            => _buffer.ReadUnsignedIntLE();

        public long DecodeLong()
            => _buffer.ReadLongLE();

        public string DecodeString(short? length = null)
            => _buffer.ReadString(length ?? _buffer.ReadShortLE(), StringEncoding);

        public Point DecodePoint()
            => new Point(_buffer.ReadShortLE(), _buffer.ReadShortLE());

        public DateTime DecodeDateTime()
            => DateTime.FromFileTimeUtc(_buffer.ReadLongLE());

        public IPacketWrite EncodeByte(byte value)
        {
            _buffer.WriteByte(value);
            return this;
        }

        public IPacketWrite EncodeBool(bool value)
        {
            _buffer.WriteByte(value ? 1 : 0);
            return this;
        }

        public IPacketWrite EncodeShort(short value)
        {
            _buffer.WriteShortLE(value);
            return this;
        }

        public IPacketWrite EncodeUShort(ushort value)
        {
            _buffer.WriteUnsignedShortLE(value);
            return this;
        }

        public IPacketWrite EncodeInt(int value)
        {
            _buffer.WriteIntLE(value);
            return this;
        }

        public IPacketWrite EncodeLong(long value)
        {
            _buffer.WriteLongLE(value);
            return this;
        }

        public IPacketWrite EncodeString(string value, short? length = null)
        {
            value ??= string.Empty;

            if (length.HasValue)
            {
                if (value.Length > length) value = value.Substring(0, length.Value);
                _buffer.WriteBytes(StringEncoding.GetBytes(value.PadRight(length.Value, '\0')));
            }
            else
            {
                _buffer.WriteShortLE(value.Length);
                _buffer.WriteBytes(StringEncoding.GetBytes(value));
            }

            return this;
        }

        public IPacketWrite EncodePoint(Point value)
        {
            _buffer.WriteShortLE(value.X);
            _buffer.WriteShortLE(value.Y);
            return this;
        }

        public IPacketWrite EncodeDateTime(DateTime value)
        {
            _buffer.WriteLongLE(value.ToFileTimeUtc());
            return this;
        }

        public void Dispose() => _buffer.DiscardReadBytes();
    }
}