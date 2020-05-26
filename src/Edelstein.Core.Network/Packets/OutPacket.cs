using System;
using System.Drawing;
using System.IO;
using System.Text;

namespace Edelstein.Core.Network.Packets
{
    public class OutPacket : IPacket, IPacketEncoder, IDisposable
    {
        private static readonly Encoding StringEncoding = Encoding.ASCII;

        private readonly MemoryStream _stream;
        private readonly BinaryWriter _writer;

        public byte[] Buffer => _stream.ToArray();
        public int Length => (int) _stream.Length;

        public OutPacket()
        {
            _stream = new MemoryStream();
            _writer = new BinaryWriter(_stream);
        }

        public OutPacket(Enum operation) : this()
            => EncodeShort(Convert.ToInt16(operation));

        public IPacketEncoder EncodeByte(byte value)
        {
            _writer.Write(value);
            return this;
        }

        public IPacketEncoder EncodeBool(bool value)
        {
            _writer.Write(value);
            return this;
        }

        public IPacketEncoder EncodeShort(short value)
        {
            _writer.Write(value);
            return this;
        }

        public IPacketEncoder EncodeUShort(ushort value)
        {
            _writer.Write(value);
            return this;
        }

        public IPacketEncoder EncodeInt(int value)
        {
            _writer.Write(value);
            return this;
        }

        public IPacketEncoder EncodeLong(long value)
        {
            _writer.Write(value);
            return this;
        }

        public IPacketEncoder EncodeString(string value, short? length = null)
        {
            value ??= string.Empty;

            if (length.HasValue)
            {
                if (value.Length > length) value = value.Substring(0, length.Value);
                _writer.Write(StringEncoding.GetBytes(value.PadRight(length.Value, '\0')));
            }
            else
            {
                _writer.Write((short) value.Length);
                _writer.Write(StringEncoding.GetBytes(value));
            }

            return this;
        }

        public IPacketEncoder EncodePoint(Point value)
        {
            EncodeShort((short) value.X);
            EncodeShort((short) value.Y);
            return this;
        }

        public IPacketEncoder EncodeDateTime(DateTime value)
        {
            EncodeLong(value.ToFileTimeUtc());
            return this;
        }

        public void Dispose()
        {
            //
        }
    }
}