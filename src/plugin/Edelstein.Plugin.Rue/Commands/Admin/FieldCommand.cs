using Edelstein.Protocol.Gameplay.Game;
using Edelstein.Protocol.Gameplay.Game.Objects.User;
using Edelstein.Protocol.Gameplay.Game.Templates;
using Edelstein.Protocol.Utilities.Templates;

namespace Edelstein.Plugin.Rue.Commands.Admin;

public class FieldCommand : AbstractTemplateCommand<IFieldTemplate>
{
    public override string Name => "Field";
    public override string Description => "Transfer to a specified field";

    private readonly IFieldManager _manager;
    private readonly ITemplateManager<IFieldStringTemplate> _strings;

    public FieldCommand(
        IFieldManager manager,
        ITemplateManager<IFieldTemplate> templates,
        ITemplateManager<IFieldStringTemplate> strings
    ) : base(templates)
    {
        _manager = manager;
        _strings = strings;

        Aliases.Add("Map");
        Aliases.Add("Warp");
    }

    protected override async Task<IReadOnlyList<TemplateCommandIndex>> Indices()
    {
        var strings = await _strings.RetrieveAll();
        var result = new TemplateCommandIndex[strings.Count * 3];
        var i = 0;

        foreach (var s in strings)
        {
            var displayName = $"{s.StreetName}: {s.MapName}";
            result[i++] = TemplateCommandIndex.CreateFromId(s.ID, displayName);
            result[i++] = TemplateCommandIndex.Create(s.ID, s.MapName, displayName);
            result[i++] = TemplateCommandIndex.Create(s.ID, s.StreetName, displayName);
        }

        return result;
    }

    protected override async Task Execute(IFieldUser user, IFieldTemplate template, TemplateCommandArgs args)
    {
        var field = await _manager.Retrieve(template.ID);
        if (field != null)
            await field.Enter(user, 0);
    }
}
