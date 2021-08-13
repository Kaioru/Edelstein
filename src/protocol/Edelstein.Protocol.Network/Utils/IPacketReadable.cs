namespace Edelstein.Protocol.Network.Utils
{
    public interface IPacketReadable
    {
        public void ReadFromPacket(IPacketReader reader);
    }
}
