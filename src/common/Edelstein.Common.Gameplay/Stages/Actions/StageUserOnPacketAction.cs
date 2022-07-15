using Edelstein.Protocol.Gameplay.Stages;
using Edelstein.Protocol.Gameplay.Stages.Messages;
using Edelstein.Protocol.Util.Pipelines;

namespace Edelstein.Common.Gameplay.Stages.Actions;

public class StageUserOnPacketAction<TStageUser> : IPipelineAction<IStageUserOnPacket<TStageUser>>
    where TStageUser : IStageUser
{
    public Task Handle(IPipelineContext ctx, IStageUserOnPacket<TStageUser> message)
    {
        Console.WriteLine($"Unhandled packet operation 0x{message.Packet.ReadShort():X}");
        return Task.CompletedTask;
    }
}
