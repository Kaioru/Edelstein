using Edelstein.Common.Utilities.Templates;
using Edelstein.Protocol.Data;
using Edelstein.Protocol.Gameplay.Game.Quests.Templates;
using Edelstein.Protocol.Utilities.Templates;

namespace Edelstein.Common.Gameplay.Game.Quests.Templates;

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
        var quest = _data.Resolve("Quest")?.ResolveAll();
        var infos = quest?.Resolve("QuestInfo.img")?.ResolveAll();
        var acts = quest?.Resolve("Act.img")?.ResolveAll();
        var checks = quest?.Resolve("Check.img")?.ResolveAll();
        
        await Task.WhenAll(infos?.Children
            .Select(async n =>
            {
                var id = Convert.ToInt32(n.Name);
                await _manager.Insert(new TemplateProviderEager<IQuestTemplate>(
                    id,
                    new QuestTemplate(
                        id,
                        n.ResolveAll(),
                        acts?.Resolve(n.Name)?.ResolveAll(),
                        checks?.Resolve(n.Name)?.ResolveAll()
                    )
                ));
            }) ?? Array.Empty<Task>());

        _manager.Freeze();
        return _manager.Count;
    }
}
