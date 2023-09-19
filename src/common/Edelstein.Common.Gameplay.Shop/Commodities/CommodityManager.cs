using Edelstein.Protocol.Gameplay.Shop.Commodities;
using Edelstein.Protocol.Gameplay.Shop.Commodities.Templates;
using Edelstein.Protocol.Utilities.Templates;

namespace Edelstein.Common.Gameplay.Shop.Commodities;

public class CommodityManager : ICommodityManager
{
    private readonly ITemplateManager<ICommodityTemplate> _templates;
    private readonly INotSaleManager _notSaleManager;
    private readonly IModifiedCommodityManager _modifiedManager;
    
    public CommodityManager(
        ITemplateManager<ICommodityTemplate> templates, 
        INotSaleManager notSaleManager, 
        IModifiedCommodityManager modifiedManager
    )
    {
        _templates = templates;
        _notSaleManager = notSaleManager;
        _modifiedManager = modifiedManager;
    }
    
    public async Task<ICommodity?> Retrieve(int key)
    {
        var template = await _templates.Retrieve(key);
        var modified = await _modifiedManager.Retrieve(key);
        
        if (template == null && modified == null) return null;
        var notSale = await _notSaleManager.Retrieve(key);
        
        return new Commodity
        {
            ID = key,
            ItemID = modified?.ItemID ?? template?.ItemID ?? 0,
            Count = modified?.Count ?? template?.Count ?? 0,
            Priority = modified?.Priority ?? template?.Priority ?? 0,
            Price = modified?.Price ?? template?.Price ?? 0,
            Bonus = modified?.Bonus ?? template?.Bonus ?? false,
            Period = modified?.Period ?? template?.Period ?? 0,
            ReqPOP = modified?.ReqPOP ?? template?.ReqPOP ?? 0,
            ReqLevel = modified?.ReqLevel ?? template?.ReqLevel ?? 0,
            MaplePoint = modified?.MaplePoint ?? template?.MaplePoint ?? 0,
            Meso = modified?.Meso ?? template?.Meso ?? 0,
            ForPremiumUser = modified?.ForPremiumUser ?? template?.ForPremiumUser ?? true,
            Gender = modified?.Gender ?? template?.Gender ?? 0,
            OnSale = notSale == null && (modified?.OnSale ?? template?.OnSale ?? false),
            Class = modified?.Class ?? template?.Class ?? 0,
            Limit = modified?.Limit ?? template?.Limit ?? 0,
            PbCash = modified?.PbCash ?? template?.PbCash ?? 0,
            PbPoint = modified?.PbPoint ?? template?.PbPoint ?? 0,
            PbGift = modified?.PbGift ?? template?.PbGift ?? 0
        };
    }
}
