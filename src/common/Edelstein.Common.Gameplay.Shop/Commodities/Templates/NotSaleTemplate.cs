using Edelstein.Protocol.Gameplay.Shop.Commodities;
using Edelstein.Protocol.Utilities.Templates;

namespace Edelstein.Common.Gameplay.Shop.Commodities.Templates;

public record NotSaleTemplate(int ID) : INotSale, ITemplate;
