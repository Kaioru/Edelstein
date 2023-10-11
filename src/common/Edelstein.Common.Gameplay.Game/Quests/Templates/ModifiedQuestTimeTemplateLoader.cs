using Duey.Abstractions;
using Edelstein.Common.Utilities.Templates;
using Edelstein.Protocol.Utilities.Templates;

namespace Edelstein.Common.Gameplay.Game.Quests.Templates;

public class ModifiedQuestTimeTemplateLoader : ITemplateLoader
{
    private readonly IDataNamespace _data;
    private readonly ITemplateManager<ModifiedQuestTimeTemplate> _manager;
    
    public ModifiedQuestTimeTemplateLoader(IDataNamespace data, ITemplateManager<ModifiedQuestTimeTemplate> manager)
    {
        _data = data;
        _manager = manager;
    }

    public async Task<int> Load()
    {
        await Task.WhenAll(_data.ResolvePath("Server/ModifiedQuestTime.img")?.Children
            .Select(async n =>
            {
                var id = Convert.ToInt32(n.Name);
                await _manager.Insert(new TemplateProviderEager<ModifiedQuestTimeTemplate>(
                    id,
                    new ModifiedQuestTimeTemplate(id, n.ResolveAll())
                ));
            }) ?? Array.Empty<Task>());

        return _manager.Count;
    }
}
