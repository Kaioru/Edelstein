using Edelstein.Protocol.Network.Packets;

namespace Edelstein.Protocol.Gameplay.Stages.Messages;

public interface ISocketOnPacket<out TStageUser> : IStageUserMessage<TStageUser> where TStageUser : IStageUser
{
    IPacketReader Packet { get; }
}
