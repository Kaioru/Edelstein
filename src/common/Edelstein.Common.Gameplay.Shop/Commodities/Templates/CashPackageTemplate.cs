using System.Collections.Frozen;
using Duey.Abstractions;
using Edelstein.Protocol.Gameplay.Shop.Commodities.Templates;

namespace Edelstein.Common.Gameplay.Shop.Commodities.Templates;

public class CashPackageTemplate : ICashPackageTemplate
{
    public int ID { get; }
    public ICollection<int> SN { get; }
    
    public CashPackageTemplate(int id, IDataNode node)
    {
        ID = id;
        SN = node
            .ResolvePath("SN")?
            .Select(c => c.ResolveInt() ?? 0)
            .ToFrozenSet() ?? FrozenSet<int>.Empty;
    }
}
