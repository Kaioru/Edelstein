using Edelstein.Core.Network;
using Edelstein.Core.Network.Packets;

namespace Edelstein.Core.Utils.Packets
{
    public interface IPacketHandlerContext
    {
        RecvPacketOperations Operation { get; }
        IPacketDecoder Packet { get; }

        ISocketAdapter Adapter { get; }
    }
}