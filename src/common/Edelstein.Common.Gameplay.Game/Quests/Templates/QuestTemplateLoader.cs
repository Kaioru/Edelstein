using Duey.Abstractions;
using Edelstein.Common.Utilities.Templates;
using Edelstein.Protocol.Gameplay.Game.Quests.Templates;
using Edelstein.Protocol.Utilities.Templates;

namespace Edelstein.Common.Gameplay.Game.Quests.Templates;

public class QuestTemplateLoader : ITemplateLoader
{
    private readonly IDataNamespace _data;
    private readonly ITemplateManager<IQuestTemplate> _manager;
    
    public QuestTemplateLoader(IDataNamespace data, ITemplateManager<IQuestTemplate> manager)
    {
        _data = data;
        _manager = manager;
    }

    public async Task<int> Load()
    {
        var quest = _data.ResolvePath("Quest")?.ResolveAll();
        var infos = quest?.ResolvePath("QuestInfo.img")?.ResolveAll();
        var acts = quest?.ResolvePath("Act.img")?.ResolveAll();
        var checks = quest?.ResolvePath("Check.img")?.ResolveAll();
        
        await Task.WhenAll(infos?.Children
            .Select(async n =>
            {
                var id = Convert.ToInt32(n.Name);
                await _manager.Insert(new TemplateProviderEager<IQuestTemplate>(
                    id,
                    new QuestTemplate(
                        id,
                        n.ResolveAll(),
                        acts?.ResolvePath(n.Name)?.ResolveAll(),
                        checks?.ResolvePath(n.Name)?.ResolveAll()
                    )
                ));
            }) ?? Array.Empty<Task>());

        return _manager.Count;
    }
}
