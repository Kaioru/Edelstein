using Edelstein.Protocol.Gameplay.Game.Contracts;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay.Game.Handling.Plugs;

public class FieldOnPacketUserMovePlug : IPipelinePlug<FieldOnPacketUserMove>
{
    public Task Handle(IPipelineContext ctx, FieldOnPacketUserMove message) =>
        message.User.Move(message.Path);
}
