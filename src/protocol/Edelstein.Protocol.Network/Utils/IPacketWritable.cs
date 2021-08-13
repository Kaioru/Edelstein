namespace Edelstein.Protocol.Network.Utils
{
    public interface IPacketWritable
    {
        public void WriteToPacket(IPacketWriter writer);
    }
}
