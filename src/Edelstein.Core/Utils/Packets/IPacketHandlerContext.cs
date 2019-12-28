using Edelstein.Network;
using Edelstein.Network.Packets;

namespace Edelstein.Core.Utils.Packets
{
    public interface IPacketHandlerContext
    {
        RecvPacketOperations Operation { get; }
        IPacket Packet { get; }

        ISocketAdapter Adapter { get; }
    }
}