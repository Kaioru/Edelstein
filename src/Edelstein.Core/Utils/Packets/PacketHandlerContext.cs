using Edelstein.Network;
using Edelstein.Network.Packets;

namespace Edelstein.Core.Utils.Packets
{
    public class PacketHandlerContext : IPacketHandlerContext
    {
        public RecvPacketOperations Operation { get; }
        public IPacket Packet { get; }
        public ISocketAdapter Adapter { get; }

        public PacketHandlerContext(RecvPacketOperations header, IPacket packet, ISocketAdapter adapter)
        {
            Operation = header;
            Packet = packet;
            Adapter = adapter;
        }
    }
}