using System;
using System.Drawing;

namespace Edelstein.Network.Packets
{
    public interface IPacketWrite
    {
        IPacketWrite EncodeByte(byte value);
        IPacketWrite EncodeBool(bool value);
        IPacketWrite EncodeShort(short value);
        IPacketWrite EncodeUShort(ushort value);
        IPacketWrite EncodeInt(int value);
        IPacketWrite EncodeLong(long value);
        IPacketWrite EncodeString(string value, short? length = null);
        IPacketWrite EncodePoint(Point value);
        IPacketWrite EncodeDateTime(DateTime value);
    }
}