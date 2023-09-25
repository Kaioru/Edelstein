using Edelstein.Common.Gameplay.Game.Objects.User.Messages;
using Edelstein.Protocol.Gameplay.Game.Objects.User;

namespace Edelstein.Plugin.Rue.Commands.Admin;

public class DebugCommand : AbstractCommand
{
    public override string Name => "Debug";
    public override string Description => "Testing command for debugging purposes";
    
    public override async Task Execute(IFieldUser user, string[] args)
    {
        await user.Message(new IncEXPMessage(1000, true));
        await user.Message(new IncMoneyMessage(1000));
        await user.Prompt(s => s.Say(user.Stats.ToString()!), default);
    }
}
