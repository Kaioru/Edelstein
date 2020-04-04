using System;
using System.Drawing;

namespace Edelstein.Network.Packets
{
    public interface IPacketEncoder
    {
        IPacketEncoder EncodeByte(byte value);
        IPacketEncoder EncodeBool(bool value);
        IPacketEncoder EncodeShort(short value);
        IPacketEncoder EncodeUShort(ushort value);
        IPacketEncoder EncodeInt(int value);
        IPacketEncoder EncodeLong(long value);
        IPacketEncoder EncodeString(string value, short? length = null);
        IPacketEncoder EncodePoint(Point value);
        IPacketEncoder EncodeDateTime(DateTime value);
    }
}