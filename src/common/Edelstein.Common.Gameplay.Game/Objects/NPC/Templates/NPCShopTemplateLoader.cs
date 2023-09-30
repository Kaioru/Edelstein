using Edelstein.Common.Utilities.Templates;
using Edelstein.Protocol.Data;
using Edelstein.Protocol.Utilities.Templates;

namespace Edelstein.Common.Gameplay.Game.Objects.NPC.Templates;

public class NPCShopTemplateLoader : ITemplateLoader
{
    private readonly IDataManager _data;
    private readonly ITemplateManager<NPCShopTemplate> _manager;
    
    public NPCShopTemplateLoader(IDataManager data, ITemplateManager<NPCShopTemplate> manager)
    {
        _data = data;
        _manager = manager;
    }

    public async Task<int> Load()
    {
        await Task.WhenAll(_data.Resolve("Server/NpcShop.img")?.Children
            .Select(async n =>
            {
                var id = Convert.ToInt32(n.Name);
                await _manager.Insert(new TemplateProviderEager<NPCShopTemplate>(
                    id,
                    new NPCShopTemplate(id, n.ResolveAll())
                ));
            }) ?? Array.Empty<Task>());

        return _manager.Count;
    }
}
