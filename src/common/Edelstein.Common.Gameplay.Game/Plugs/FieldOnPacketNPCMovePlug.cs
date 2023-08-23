using Edelstein.Protocol.Gameplay.Game.Contracts;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay.Game.Plugs;

public class FieldOnPacketNPCMovePlug : IPipelinePlug<FieldOnPacketNPCMove>
{
    public Task Handle(IPipelineContext ctx, FieldOnPacketNPCMove message) =>
        message.NPC.Move(message.Path);
}
