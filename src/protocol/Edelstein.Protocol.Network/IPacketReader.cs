using System;
using Edelstein.Protocol.Util.Spatial;

namespace Edelstein.Protocol.Network
{
    public interface IPacketReader : IPacket
    {
        long Cursor { get; }
        long Available { get; }

        byte ReadByte();
        bool ReadBool();

        short ReadShort();
        ushort ReadUShort();

        int ReadInt();
        uint ReadUInt();

        long ReadLong();
        ulong ReadULong();

        string ReadString(short? length = null);
        byte[] ReadBytes(short length);

        Point2D ReadPoint2D();

        DateTime ReadDateTime();
    }
}
