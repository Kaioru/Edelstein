using System.Threading.Tasks;
using Edelstein.Protocol.Gameplay;
using Edelstein.Protocol.Gameplay.Handling;
using Edelstein.Protocol.Network.Packets;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay.Handling;

public class PipedPacketHandler<TStageSystem, TStageSystemUser, TMessage>(
    IPipeline<PipedPacketMessage<TStageSystem, TStageSystemUser, TMessage>> pipeline
) : IPacketHandler<TStageSystem, TStageSystemUser>
    where TStageSystem : IStageSystem<TStageSystem, TStageSystemUser>
    where TStageSystemUser : IStageSystemUser<TStageSystem, TStageSystemUser>
    where TMessage : StructuredBasePacket
{
    public Task Handle(TStageSystemUser user, IRawPacket packet)
    {
        using var reader = new RawPacketReader(packet);
        var message = reader.ReadStructured<TMessage>();

        return pipeline.Process(new PipedPacketMessage<TStageSystem, TStageSystemUser, TMessage>(user, message));
    }
}
