using System.Threading.Tasks;
using Edelstein.Protocol.Gameplay;
using Edelstein.Protocol.Gameplay.Handling;
using Edelstein.Protocol.Network.Packets;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay.Handling;

public class PipedPacketHandler<TStageSystemUser, TStageSystem, TMessage>(
    short operation,
    IPipeline<PipedPacketMessage<TStageSystemUser, TMessage>> pipeline
) : IPacketHandler<TStageSystemUser, TStageSystem>
    where TStageSystemUser : IStageSystemUser<TStageSystemUser, TStageSystem>
    where TStageSystem : IStageSystem<TStageSystemUser, TStageSystem>
    where TMessage : StructuredBasePacket
{
    public short Operation { get; } = operation;
    
    public Task Handle(TStageSystemUser user, IRawPacket packet)
    {
        using var reader = new RawPacketReader(packet);
        var message = reader.ReadStructured<TMessage>();

        return pipeline.Process(new PipedPacketMessage<TStageSystemUser, TMessage>(user, message));
    }
}
