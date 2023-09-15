using Edelstein.Common.Utilities.Templates;
using Edelstein.Protocol.Data;
using Edelstein.Protocol.Utilities.Templates;

namespace Edelstein.Common.Gameplay.Shop.Commodities.Templates;

public class NotSaleTemplateLoader : ITemplateLoader
{
    private readonly IDataManager _data;
    private readonly ITemplateManager<NotSaleTemplate> _manager;
    
    public NotSaleTemplateLoader(IDataManager data, ITemplateManager<NotSaleTemplate> manager)
    {
        _data = data;
        _manager = manager;
    }

    public async Task<int> Load()
    {
        await Task.WhenAll(_data.Resolve("Server/NotSale.img")?.Children
            .Select(async n =>
            {
                var sn = n.Resolve<int>() ?? 0;
                await _manager.Insert(new TemplateProviderEager<NotSaleTemplate>(
                    sn,
                    new NotSaleTemplate(sn)
                ));
            }) ?? Array.Empty<Task>());

        return _manager.Count;
    }
}
