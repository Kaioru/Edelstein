using System.Collections.Immutable;
using Edelstein.Protocol.Gameplay.Shop.Commodities;
using Edelstein.Protocol.Gameplay.Shop.Commodities.Templates;
using Edelstein.Protocol.Utilities.Templates;

namespace Edelstein.Common.Gameplay.Shop.Commodities;

public class CashPackageManager : ICashPackageManager
{
    private readonly ITemplateManager<ICashPackageTemplate> _templates;
    private readonly IModifiedCommodityManager _modifiedManager;

    public CashPackageManager(ITemplateManager<ICashPackageTemplate> templates, IModifiedCommodityManager modifiedManager)
    {
        _templates = templates;
        _modifiedManager = modifiedManager;
    }
    
    public async Task<ICashPackage?> Retrieve(int key)
    {
        var template = await _templates.Retrieve(key);
        var modified = (await _modifiedManager.RetrieveAll())
            .FirstOrDefault(m => m.ItemID == key);
        
        if (template == null && modified?.PackageSN == null) return null;
        
        return new CashPackage
        {
            ID = key,
            SN = modified?.PackageSN ?? template?.SN ?? ImmutableList<int>.Empty
        };
    }
}
