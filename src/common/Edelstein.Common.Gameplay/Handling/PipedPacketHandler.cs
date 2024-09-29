using System;
using System.Threading.Tasks;
using Edelstein.Protocol.Gameplay;
using Edelstein.Protocol.Gameplay.Handling;
using Edelstein.Protocol.Network.Packets;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay.Handling;

public class PipedPacketHandler<TStageSystem, TStageSystemUser, TMessage>(
    short operation,
    IPipeline<PipedPacketMessage<TStageSystem, TStageSystemUser, TMessage>> pipeline
) : IPacketHandlerManagerEntry<TStageSystem, TStageSystemUser>
    where TStageSystem : IStageSystem<TStageSystem, TStageSystemUser>
    where TStageSystemUser : class, IStageSystemUser<TStageSystem, TStageSystemUser>
    where TMessage : StructuredBasePacket
{
    public short Operation { get; } = operation;
    
    public Task Handle(TStageSystemUser user, IRawPacket packet)
    {
        try
        {
            using var reader = new RawPacketReader(packet);
            var message = reader.ReadStructured<TMessage>();

            return pipeline.Process(new PipedPacketMessage<TStageSystem, TStageSystemUser, TMessage>(user, message));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return Task.CompletedTask;
        }
    }
}
