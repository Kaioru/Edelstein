using System;

namespace Edelstein.Protocol.Network.Packets;

public interface IRawPacketWriter : IDispatchable, IDisposable
{
    long Length { get; }

    IRawPacketWriter WriteByte(byte value);
    IRawPacketWriter WriteBool(bool value);

    IRawPacketWriter WriteShort(short value);
    IRawPacketWriter WriteUShort(ushort value);

    IRawPacketWriter WriteInt(int value);
    IRawPacketWriter WriteUInt(uint value);

    IRawPacketWriter WriteLong(long value);
    IRawPacketWriter WriteULong(ulong value);

    IRawPacketWriter WriteDouble(double value);

    IRawPacketWriter WriteString(string value, short? length = null);
    IRawPacketWriter WriteBytes(byte[] value);
}
