using Edelstein.Protocol.Gameplay.Game.Objects.NPC.Templates;
using Edelstein.Protocol.Gameplay.Game.Objects.User;
using Edelstein.Protocol.Utilities.Templates;

namespace Edelstein.Plugin.Rue.Commands.Admin;

public class NPCCommand : AbstractTemplateCommand<INPCTemplate>
{
    private readonly ITemplateManager<INPCStringTemplate> _strings;

    public NPCCommand(
        ITemplateManager<INPCTemplate> templates,
        ITemplateManager<INPCStringTemplate> strings
    ) : base(templates)
        => _strings = strings;

    public override string Name => "NPC";
    public override string Description => "Searches a specified NPC";

    protected override async Task<IReadOnlyList<TemplateCommandIndex>> Indices()
    {
        var strings = await _strings.RetrieveAll();
        var result = new TemplateCommandIndex[strings.Count * 3];
        var i = 0;

        foreach (var s in strings)
        {
            var displayName = string.IsNullOrWhiteSpace(s.Func) ? s.Name : $"{s.Name}: {s.Func}";
            result[i++] = TemplateCommandIndex.CreateFromId(s.ID, s.Name);
            result[i++] = TemplateCommandIndex.Create(s.ID, s.Name, displayName);
            result[i++] = TemplateCommandIndex.Create(s.ID, s.Func ?? string.Empty, displayName);
        }

        return result;
    }

    protected override Task Execute(IFieldUser user, INPCTemplate template, TemplateCommandArgs args)
        => Task.CompletedTask;
}
