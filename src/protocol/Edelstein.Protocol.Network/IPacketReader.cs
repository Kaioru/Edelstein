using System;
using System.Numerics;
using Edelstein.Protocol.Network.Utils;
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

        T Read<T>(T readable) where T : IPacketReadable;
    }
}
