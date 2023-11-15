using Duey.Abstractions;
using Edelstein.Common.Utilities.Templates;
using Edelstein.Protocol.Gameplay.Shop.Commodities.Templates;
using Edelstein.Protocol.Utilities.Templates;

namespace Edelstein.Common.Gameplay.Shop.Commodities.Templates;

public class CashPackageTemplateLoader : ITemplateLoader
{
    private readonly IDataNamespace _data;
    private readonly ITemplateManager<ICashPackageTemplate> _manager;
    
    public CashPackageTemplateLoader(IDataNamespace data, ITemplateManager<ICashPackageTemplate> manager)
    {
        _data = data;
        _manager = manager;
    }

    public async Task<int> Load()
    {
        await Task.WhenAll(_data.ResolvePath("Etc/CashPackage.img")?.Children
            .Select(async n =>
            {
                var sn = Convert.ToInt32(n.Name);
                await _manager.Insert(new TemplateProviderLazy<ICashPackageTemplate>(
                    sn,
                    () => new CashPackageTemplate(sn, n.Cache())
                ));
            }) ?? Array.Empty<Task>());

        _manager.Freeze();
        return _manager.Count;
    }
}
