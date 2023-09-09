using Edelstein.Plugin.Rue.Commands;
using Edelstein.Protocol.Gameplay.Game.Contracts;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Plugin.Rue.Plugs;

public class FieldOnPacketUserChatCommandPlug : IPipelinePlug<FieldOnPacketUserChat>
{
    private readonly ICommandManager _commandManager;
    
    public FieldOnPacketUserChatCommandPlug(ICommandManager commandManager) => _commandManager = commandManager;
    
    public async Task Handle(IPipelineContext ctx, FieldOnPacketUserChat message)
    {
        if (message.Message.StartsWith("!") || message.Message.StartsWith("@"))
        {
            await _commandManager.Process(message.User, message.Message[1..]);
            ctx.Cancel();
        }
    }
}
