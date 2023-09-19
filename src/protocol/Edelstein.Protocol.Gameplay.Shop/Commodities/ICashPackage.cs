using Edelstein.Protocol.Utilities.Repositories;

namespace Edelstein.Protocol.Gameplay.Shop.Commodities;

public interface ICashPackage : IIdentifiable<int>
{
    ICollection<int> SN { get; }
}
