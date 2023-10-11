using Duey.Abstractions;
using Edelstein.Common.Utilities.Templates;
using Edelstein.Protocol.Gameplay.Shop.Commodities.Templates;
using Edelstein.Protocol.Utilities.Templates;

namespace Edelstein.Common.Gameplay.Shop.Commodities.Templates;

public class CommodityTemplateLoader : ITemplateLoader
{
    private readonly IDataNamespace _data;
    private readonly ITemplateManager<ICommodityTemplate> _manager;
    
    public CommodityTemplateLoader(IDataNamespace data, ITemplateManager<ICommodityTemplate> manager)
    {
        _data = data;
        _manager = manager;
    }

    public async Task<int> Load()
    {
        await Task.WhenAll(_data.ResolvePath("Etc/Commodity.img")?.Children
            .Select(async n =>
            {
                await _manager.Insert(new TemplateProviderLazy<ICommodityTemplate>(
                    n.ResolveInt("SN")!.Value,
                    () => new CommodityTemplate(n.Cache())
                ));
            }) ?? Array.Empty<Task>());

        return _manager.Count;
    }
}
