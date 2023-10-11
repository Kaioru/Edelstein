using Duey.Abstractions;
using Edelstein.Common.Utilities.Templates;
using Edelstein.Protocol.Utilities.Templates;

namespace Edelstein.Common.Gameplay.Shop.Commodities.Templates;

public class ModifiedCommodityTemplateLoader : ITemplateLoader
{
    private readonly IDataNamespace _data;
    private readonly ITemplateManager<ModifiedCommodityTemplate> _manager;
    
    public ModifiedCommodityTemplateLoader(IDataNamespace data, ITemplateManager<ModifiedCommodityTemplate> manager)
    {
        _data = data;
        _manager = manager;
    }

    public async Task<int> Load()
    {
        await Task.WhenAll(_data.ResolvePath("Server/ModifiedCommodity.img")?.Children
            .Select(async n =>
            {
                var sn = Convert.ToInt32(n.Name);
                await _manager.Insert(new TemplateProviderEager<ModifiedCommodityTemplate>(
                    sn,
                    new ModifiedCommodityTemplate(sn, n.Cache())
                ));
            }) ?? Array.Empty<Task>());

        return _manager.Count;
    }
}
