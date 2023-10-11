using Duey.Abstractions;
using Edelstein.Common.Utilities.Templates;
using Edelstein.Protocol.Utilities.Templates;

namespace Edelstein.Common.Gameplay.Game.Objects.NPC.Templates;

public class NPCShopTemplateLoader : ITemplateLoader
{
    private readonly IDataNamespace _data;
    private readonly ITemplateManager<NPCShopTemplate> _manager;
    
    public NPCShopTemplateLoader(IDataNamespace data, ITemplateManager<NPCShopTemplate> manager)
    {
        _data = data;
        _manager = manager;
    }

    public async Task<int> Load()
    {
        await Task.WhenAll(_data.ResolvePath("Server/NpcShop.img")?.Children
            .Select(async n =>
            {
                var id = Convert.ToInt32(n.Name);
                await _manager.Insert(new TemplateProviderEager<NPCShopTemplate>(
                    id,
                    new NPCShopTemplate(id, n.Cache())
                ));
            }) ?? Array.Empty<Task>());

        return _manager.Count;
    }
}
