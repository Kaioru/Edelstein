namespace Edelstein.Protocol.Utilities.Packets;

public interface IPacket : IDisposable
{
    int Length { get; }
    byte[] Buffer { get; }
}
