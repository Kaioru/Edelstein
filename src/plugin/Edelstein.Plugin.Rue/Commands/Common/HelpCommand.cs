using Edelstein.Protocol.Gameplay.Game.Objects.User;

namespace Edelstein.Plugin.Rue.Commands.Common;

public class HelpCommand : AbstractCommand
{
    public override string Name => "Help";
    public override string Description => "Shows all available commands";

    private readonly ICommandManager _processor;

    public HelpCommand(ICommandManager processor)
    {
        _processor = processor;

        Aliases.Add("Commands");
        Aliases.Add("?");
    }

    public override async Task Execute(IFieldUser user, string[] args)
    {
        await user.Message("Available commands:");
        await Task.WhenAll((await _processor.RetrieveAll())
            .Where(c => c.Check(user))
            .Select(c => user.Message($"{c.Name} - {c.Description}")));
    }
}
