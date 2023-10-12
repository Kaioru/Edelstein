using Edelstein.Protocol.Gameplay.Game.Contracts;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay.Game.Handling.Plugs;

public class FieldOnPacketDragonMovePlug : IPipelinePlug<FieldOnPacketDragonMove>
{
    public Task Handle(IPipelineContext ctx, FieldOnPacketDragonMove message)
        => message.Dragon.Move(message.Path, message.User);
}
