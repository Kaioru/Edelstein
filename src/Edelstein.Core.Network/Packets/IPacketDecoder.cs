using System;
using System.Drawing;

namespace Edelstein.Network.Packets
{
    public interface IPacketDecoder
    {
        byte DecodeByte();
        bool DecodeBool();
        short DecodeShort();
        ushort DecodeUShort();
        int DecodeInt();
        uint DecodeUInt();
        long DecodeLong();
        string DecodeString(short? length = null);
        Point DecodePoint();
        DateTime DecodeDateTime();
    }
}