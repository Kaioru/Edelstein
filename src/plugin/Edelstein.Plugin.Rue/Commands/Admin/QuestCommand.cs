using Edelstein.Protocol.Gameplay.Game.Objects.User;
using Edelstein.Protocol.Gameplay.Game.Quests.Templates;
using Edelstein.Protocol.Utilities.Templates;

namespace Edelstein.Plugin.Rue.Commands.Admin;

public class QuestCommand : AbstractTemplateCommand<IQuestTemplate>
{
    public override string Name => "Quest";
    public override string Description => "Searches a specified quest";

    private readonly ITemplateManager<IQuestTemplate> _templates;

    public QuestCommand(ITemplateManager<IQuestTemplate> templates) : base(templates)
        => _templates = templates;

    protected override async Task<IReadOnlyList<TemplateCommandIndex>> Indices()
    {
        var strings = await _templates.RetrieveAll();
        var result = new TemplateCommandIndex[strings.Count * 2];
        var i = 0;

        foreach (var s in strings)
        {
            result[i++] = TemplateCommandIndex.CreateFromId(s.ID, s.Name);
            result[i++] = TemplateCommandIndex.Create(s.ID, s.Name, s.Name);
        }

        return result;
    }

    protected override Task Execute(IFieldUser user, IQuestTemplate template, TemplateCommandArgs args)
        => Task.CompletedTask;
}
