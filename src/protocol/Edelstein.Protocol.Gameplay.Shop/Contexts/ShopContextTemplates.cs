using Edelstein.Protocol.Gameplay.Models.Inventories.Templates;
using Edelstein.Protocol.Gameplay.Shop.Commodities.Templates;
using Edelstein.Protocol.Utilities.Templates;

namespace Edelstein.Protocol.Gameplay.Shop.Contexts;

public record ShopContextTemplates(
    ITemplateManager<IItemTemplate> Item,
    ITemplateManager<IItemStringTemplate> ItemString,
    ITemplateManager<ICommodityTemplate> Commodity
);
