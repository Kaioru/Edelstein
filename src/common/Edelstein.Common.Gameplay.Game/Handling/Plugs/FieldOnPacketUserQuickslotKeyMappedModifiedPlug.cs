using Edelstein.Protocol.Gameplay.Game.Contracts;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay.Game.Handling.Plugs;

public class FieldOnPacketUserQuickslotKeyMappedModifiedPlug : IPipelinePlug<FieldOnPacketUserQuickslotKeyMappedModified>
{
    public Task Handle(IPipelineContext ctx, FieldOnPacketUserQuickslotKeyMappedModified message)
    {
        message.User.Character.QuickslotKeys.Records.Clear();
        foreach (var key in message.Keys)
            message.User.Character.QuickslotKeys.Records[key.Key] = key.Value;
        return Task.CompletedTask;
    }
}
