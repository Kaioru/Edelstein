using Duey.Abstractions;
using Edelstein.Common.Utilities.Templates;
using Edelstein.Protocol.Utilities.Templates;

namespace Edelstein.Common.Gameplay.Shop.Commodities.Templates;

public class NotSaleTemplateLoader : ITemplateLoader
{
    private readonly IDataNamespace _data;
    private readonly ITemplateManager<NotSaleTemplate> _manager;
    
    public NotSaleTemplateLoader(IDataNamespace data, ITemplateManager<NotSaleTemplate> manager)
    {
        _data = data;
        _manager = manager;
    }

    public async Task<int> Load()
    {
        await Task.WhenAll(_data.ResolvePath("Server/NotSale.img")?.Children
            .Select(async n =>
            {
                var sn = n.ResolveInt() ?? 0;
                await _manager.Insert(new TemplateProviderEager<NotSaleTemplate>(
                    sn,
                    new NotSaleTemplate(sn)
                ));
            }) ?? Array.Empty<Task>());

        _manager.Freeze();
        return _manager.Count;
    }
}
