using Edelstein.Protocol.Gameplay.Stages.Game.Objects.NPC.Contracts;
using Edelstein.Protocol.Util.Pipelines;

namespace Edelstein.Common.Gameplay.Stages.Game.Objects.NPC.Plugs;

public class NPCMovePlug : IPipelinePlug<INPCMove>
{
    public Task Handle(IPipelineContext ctx, INPCMove message) =>
        message.NPC.Move(message.Path);
}
