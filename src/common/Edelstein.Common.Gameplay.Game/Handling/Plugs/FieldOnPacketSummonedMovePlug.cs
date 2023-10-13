using Edelstein.Protocol.Gameplay.Game.Contracts;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay.Game.Handling.Plugs;

public class FieldOnPacketSummonedMovePlug : IPipelinePlug<FieldOnPacketSummonedMove>
{
    public Task Handle(IPipelineContext ctx, FieldOnPacketSummonedMove message)
        => message.Summoned.Move(message.Path, message.User);
}
