using Edelstein.Protocol.Util.Buffers.Packets;

namespace Edelstein.Protocol.Gameplay.Stages.Contracts;

public interface ISocketOnPacket<out TStageUser> : IStageUserMessage<TStageUser>
    where TStageUser : IStageUser<TStageUser>
{
    IPacket Packet { get; }
}
