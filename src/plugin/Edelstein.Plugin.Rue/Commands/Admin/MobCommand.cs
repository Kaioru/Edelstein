using Edelstein.Protocol.Gameplay.Game.Objects.Mob.Templates;
using Edelstein.Protocol.Gameplay.Game.Objects.User;
using Edelstein.Protocol.Utilities.Templates;

namespace Edelstein.Plugin.Rue.Commands.Admin;

public class MobCommand : AbstractTemplateCommand<IMobTemplate>
{
    private readonly ITemplateManager<IMobStringTemplate> _strings;
    
    public MobCommand(
        ITemplateManager<IMobTemplate> templates, 
        ITemplateManager<IMobStringTemplate> strings
    ) : base(templates) 
        => _strings = strings;

    public override string Name => "Mob";
    public override string Description => "Searches a specified mob";
    
    protected override async Task<IEnumerable<TemplateCommandIndex>> Indices()
    {
        var result = new List<TemplateCommandIndex>();
        var strings = (await _strings.RetrieveAll()).ToList();

        result.AddRange(strings.Select(s => new TemplateCommandIndex(s.ID, s.ID.ToString(), s.Name)));
        result.AddRange(strings.Select(s => new TemplateCommandIndex(s.ID, s.Name, s.Name)));

        return result;
    }
    
    protected override Task Execute(IFieldUser user, IMobTemplate template, TemplateCommandArgs args)
        => Task.CompletedTask;
}
