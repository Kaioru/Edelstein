using Edelstein.Common.Utilities.Templates;
using Edelstein.Protocol.Data;
using Edelstein.Protocol.Gameplay.Models.Inventories.Templates;
using Edelstein.Protocol.Gameplay.Shop.Commodities.Templates;
using Edelstein.Protocol.Utilities.Templates;

namespace Edelstein.Common.Gameplay.Shop.Commodities.Templates;

public class CommodityTemplateLoader : ITemplateLoader
{
    private readonly IDataManager _data;
    private readonly ITemplateManager<ICommodityTemplate> _manager;
    
    public CommodityTemplateLoader(IDataManager data, ITemplateManager<ICommodityTemplate> manager)
    {
        _data = data;
        _manager = manager;
    }

    public async Task<int> Load()
    {
        await Task.WhenAll(_data.Resolve("Etc/Commodity.img")?.Children
            .Select(async n =>
            {
                await _manager.Insert(new TemplateProviderLazy<ICommodityTemplate>(
                    n.Resolve<int>("SN")!.Value,
                    () => new CommodityTemplate(n.ResolveAll())
                ));
            }) ?? Array.Empty<Task>());

        return _manager.Count;
    }
}
