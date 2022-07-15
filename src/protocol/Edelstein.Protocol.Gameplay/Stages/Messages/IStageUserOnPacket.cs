using Edelstein.Protocol.Network.Packets;

namespace Edelstein.Protocol.Gameplay.Stages.Messages;

public interface IStageUserOnPacket<out TStageUser> : IStageUserRequest<TStageUser> where TStageUser : IStageUser
{
    IPacketReader Packet { get; }
}
