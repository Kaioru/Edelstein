using Edelstein.Protocol.Util.Buffers.Bytes;

namespace Edelstein.Protocol.Gameplay.Stages.Messages;

public interface ISocketOnPacket<out TStageUser> : IStageUserMessage<TStageUser>
    where TStageUser : IStageUser<TStageUser>
{
    IPacket Packet { get; }
}
