using Edelstein.Plugin.Rue.Commands;
using Edelstein.Protocol.Gameplay.Game.Contracts;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Plugin.Rue.Plugs;

public class FieldOnPacketUserChatCommandPlug(ICommandManager commandManager) : IPipelinePlug<FieldOnPacketUserChat>
{
    private readonly ICommandManager _commandManager = commandManager;

    public Task Handle(IPipelineContext ctx, FieldOnPacketUserChat message)
    {
        if (message.Message.StartsWith('!') || message.Message.StartsWith('@'))
        {
            _ = Task.Run(() => _commandManager.Process(message.User, message.Message[1..]));
            ctx.Cancel();
        }

        return Task.CompletedTask;
    }
}
