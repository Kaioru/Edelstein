namespace Edelstein.Network.Packets
{
    public interface IPacket
    {
        byte[] Buffer { get; }
        int Length { get; }
    }
}