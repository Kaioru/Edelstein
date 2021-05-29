using System;
using System.Numerics;

namespace Edelstein.Protocol.Network
{
    public interface IPacketWriter : IPacket
    {
        long Length { get; }

        IPacketWriter WriteByte(byte value);
        IPacketWriter WriteBool(bool value);

        IPacketWriter WriteShort(short value);
        IPacketWriter WriteUShort(ushort value);

        IPacketWriter WriteInt(int value);
        IPacketWriter WriteUInt(uint value);

        IPacketWriter WriteLong(long value);
        IPacketWriter WriteULong(ulong value);

        IPacketWriter WriteString(string value, short? length = null);
        IPacketWriter WriteBytes(byte[] value);

        IPacketWriter WriteVector2(Vector2 value);
        IPacketWriter WriteDateTime(DateTime value);
    }
}