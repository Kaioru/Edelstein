using System;
using System.Numerics;

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

        Vector2 ReadVector2();
        DateTime ReadDateTime();
    }
}
