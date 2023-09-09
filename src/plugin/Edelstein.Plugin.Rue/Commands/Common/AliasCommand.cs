using Edelstein.Protocol.Gameplay.Game.Objects.User;

namespace Edelstein.Plugin.Rue.Commands.Common;

public class AliasCommand : AbstractCommand
{
    public override string Name => "Alias";
    public override string Description => "Shows all available command aliases";

    private readonly ICommandManager _processor;

    public AliasCommand(ICommandManager processor)
    {
        _processor = processor;

        Aliases.Add("Aliases");
    }

    public override async Task Execute(IFieldUser user, string[] args)
    {
        await user.Message("Available command aliases:");
        await Task.WhenAll((await _processor.RetrieveAll())
            .Where(c => c.Check(user))
            .Where(c => c.Aliases.Count > 0)
            .Select(c => user.Message($"{c.Name} - {string.Join(", ", c.Aliases)}")));
    }
}
