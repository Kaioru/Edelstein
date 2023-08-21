namespace Edelstein.Protocol.Utilities.Packets;

public interface IPacketReader : IDisposable
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

    IPacketReader Skip(short length);
}
