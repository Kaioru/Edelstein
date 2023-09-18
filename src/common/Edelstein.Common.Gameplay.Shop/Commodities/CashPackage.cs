using Edelstein.Protocol.Gameplay.Shop.Commodities;

namespace Edelstein.Common.Gameplay.Shop.Commodities;

public record CashPackage : ICashPackage
{
    public int ID { get; set; }
    
    public ICollection<int> SN { get; set; }
}
