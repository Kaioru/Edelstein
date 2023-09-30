using System.Collections.Frozen;
using Edelstein.Protocol.Data;
using Edelstein.Protocol.Gameplay.Shop.Commodities.Templates;

namespace Edelstein.Common.Gameplay.Shop.Commodities.Templates;

public class CashPackageTemplate : ICashPackageTemplate
{
    public int ID { get; }
    public ICollection<int> SN { get; }
    
    public CashPackageTemplate(int id, IDataProperty property)
    {
        ID = id;
        SN = property
            .Resolve("SN")?
            .Select(c => c.Resolve<int>() ?? 0)
            .ToFrozenSet() ?? FrozenSet<int>.Empty;
    }
}
