using System;
using System.Collections.Generic;
using DotNetty.Buffers;

namespace Edelstein.Network.Packets
{
    public class Packet : IPacket
    {
        private readonly IByteBuffer _buffer;

        public byte[] Buffer => _buffer.Array;
        public int Length => _buffer.ReadableBytes;

        public Packet(IByteBuffer buffer)
            => _buffer = buffer;

        public Packet(Enum operation, int initialCapacity = byte.MaxValue)
            : this(Unpooled.Buffer(initialCapacity))
            => Encode<short>(Convert.ToInt16(operation));

        public Packet(int initialCapacity = byte.MaxValue)
            : this(Unpooled.Buffer(initialCapacity))
        {
        }

        public IPacket Encode<T>(T value)
        {
            var type = typeof(T);
            if (value == null) value = default;
            PacketMethods.EncodeMethods.TryGetValue(type, out var method);
            if (method == null) throw new NotSupportedException();
            method(_buffer, value);
            return this;
        }

        public IPacket EncodeFixedString(string value, int length)
        {
            if (value == null) value = string.Empty;
            if (value.Length > length) value = value.Substring(0, length);
            _buffer.WriteBytes(PacketMethods.StringEncoding.GetBytes(value.PadRight(length, '\0')));
            return this;
        }

        public T Decode<T>()
        {
            var type = typeof(T);
            PacketMethods.DecodeMethods.TryGetValue(type, out var method);
            if (method == null) throw new NotSupportedException();
            return (T) method(_buffer);
        }

        public IEnumerable<byte> DecodeFixedLength(int length)
            => _buffer.ReadBytes(length).Array;

        public void Dispose() => _buffer.DiscardReadBytes();
    }
}