using Edelstein.Protocol.Gameplay.Models.Inventories.Templates;
using Edelstein.Protocol.Utilities.Templates;

namespace Edelstein.Protocol.Gameplay.Trade.Contexts;

public record TradeContextTemplates(
    ITemplateManager<IItemTemplate> Item,
    ITemplateManager<IItemStringTemplate> ItemString
);
