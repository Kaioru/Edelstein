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
    
    protected override async Task<IEnumerable<TemplateCommandIndex>> Indices()
    {
        var result = new List<TemplateCommandIndex>();
        var strings = await _strings.RetrieveAll();

        result.AddRange(strings.Select(s => new TemplateCommandIndex(s.ID, s.ID.ToString(), s.Name)));
        result.AddRange(strings.Select(s => new TemplateCommandIndex(s.ID, s.Name, $"{s.Name}{(!string.IsNullOrWhiteSpace(s.Func) ? $": {s.Func}" : "")}")));
        result.AddRange(strings.Select(s => new TemplateCommandIndex(s.ID, s.Func, $"{s.Name}{(!string.IsNullOrWhiteSpace(s.Func) ? $": {s.Func}" : "")}")));

        return result;
    }
    
    protected override Task Execute(IFieldUser user, INPCTemplate template, TemplateCommandArgs args)
        => Task.CompletedTask;
}
