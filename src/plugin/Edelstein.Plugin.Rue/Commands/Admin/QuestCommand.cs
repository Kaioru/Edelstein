using Edelstein.Protocol.Gameplay.Game.Objects.User;
using Edelstein.Protocol.Gameplay.Models.Characters.Quests.Templates;
using Edelstein.Protocol.Gameplay.Models.Inventories.Templates;
using Edelstein.Protocol.Utilities.Templates;

namespace Edelstein.Plugin.Rue.Commands.Admin;

public class QuestCommand : AbstractTemplateCommand<IQuestTemplate>
{
    public override string Name => "Quest";
    public override string Description => "Searches a specified quest";

    private readonly ITemplateManager<IQuestTemplate> _strings;

    public QuestCommand(
        ITemplateManager<IQuestTemplate> templates
    ) : base(templates)
    {
        _strings = templates;
    }

    protected override async Task<IEnumerable<TemplateCommandIndex>> Indices()
    {
        var result = new List<TemplateCommandIndex>();
        var strings = (await _strings.RetrieveAll()).ToList();

        result.AddRange(strings.Select(s => new TemplateCommandIndex(s.ID, s.ID.ToString(), s.Name)));
        result.AddRange(strings.Select(s => new TemplateCommandIndex(s.ID, s.Name, s.Name)));

        return result;
    }

    protected override Task Execute(IFieldUser user, IQuestTemplate template, TemplateCommandArgs args)
        => Task.CompletedTask;
}
