using System;

namespace Edelstein.Protocol.Network.Packets;

public interface IRawPacketReader : IDispatchable, IDisposable
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

    double ReadDouble();

    string ReadString(short? length = null);
    byte[] ReadBytes(short length);

    T ReadStructured<T>() where T : StructuredBasePacket;

    IRawPacketReader Skip(short length);
}
