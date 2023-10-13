using Edelstein.Protocol.Gameplay.Game.Contracts;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay.Game.Handling.Plugs;

public class FieldOnPacketUserFuncKeyMappedModifiedPlug : IPipelinePlug<FieldOnPacketUserFuncKeyMappedModified>
{
    public Task Handle(IPipelineContext ctx, FieldOnPacketUserFuncKeyMappedModified message)
    {
        foreach (var kv in message.Keys)
            message.User.Character.FuncKeys.Records[kv.Key] = kv.Value;
        return Task.CompletedTask;
    }
}
