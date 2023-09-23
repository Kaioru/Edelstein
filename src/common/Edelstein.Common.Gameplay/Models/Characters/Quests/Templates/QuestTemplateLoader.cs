using Edelstein.Common.Gameplay.Models.Characters.Skills.Templates;
using Edelstein.Common.Utilities.Templates;
using Edelstein.Protocol.Data;
using Edelstein.Protocol.Gameplay.Models.Characters.Quests.Templates;
using Edelstein.Protocol.Gameplay.Models.Characters.Skills.Templates;
using Edelstein.Protocol.Utilities.Templates;

namespace Edelstein.Common.Gameplay.Models.Characters.Quests.Templates;

public class QuestTemplateLoader : ITemplateLoader
{
    private readonly IDataManager _data;
    private readonly ITemplateManager<IQuestTemplate> _manager;
    
    public QuestTemplateLoader(IDataManager data, ITemplateManager<IQuestTemplate> manager)
    {
        _data = data;
        _manager = manager;
    }

    public async Task<int> Load()
    {
        await Task.WhenAll(_data.Resolve("Quest/QuestData")?.Children
            .Where(c => c.Name.Split(".")[0].All(char.IsDigit))
            .Select(async n =>
            {
                var id = Convert.ToInt32(n.Name.Split(".")[0]);
                await _manager.Insert(new TemplateProviderEager<IQuestTemplate>(
                    id,
                    new QuestTemplate(
                        id,
                        n.ResolveAll()
                    )
                ));
            }) ?? Array.Empty<Task>());

        return _manager.Count;
    }
}
