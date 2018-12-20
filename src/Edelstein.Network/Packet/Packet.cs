using System;
using DotNetty.Buffers;

namespace Edelstein.Network.Packet
{
    public class Packet : IPacket
    {
        private readonly IByteBuffer _byteBuffer;
        public byte[] Buffer => _byteBuffer.Array;
        public int Length => _byteBuffer.ReadableBytes;

        public Packet(IByteBuffer byteBuffer)
            => _byteBuffer = byteBuffer;

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
            if (value == null) value = default(T);
            if (!PacketMethods.EncodeMethods.ContainsKey(type))
                throw new NotSupportedException();
            PacketMethods.EncodeMethods[type](_byteBuffer, value);
            return this;
        }

        public IPacket EncodeFixedString(string value, int length)
        {
            if (value.Length > length) value = value.Substring(0, length);
            _byteBuffer.WriteBytes(PacketMethods.StringEncoding.GetBytes(value.PadRight(length, '\0')));
            return this;
        }

        public T Decode<T>()
        {
            var type = typeof(T);
            if (PacketMethods.DecodeMethods.ContainsKey(type))
                return (T) PacketMethods.DecodeMethods[type](_byteBuffer);
            throw new NotSupportedException();
        }

        public void Dispose()
            => _byteBuffer.DiscardReadBytes();
    }
}