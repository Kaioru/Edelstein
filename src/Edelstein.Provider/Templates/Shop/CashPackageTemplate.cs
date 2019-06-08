using System.Linq;
using Edelstein.Provider.Parsing;

namespace Edelstein.Provider.Templates.Shop
{
    public class CashPackageTemplate : ITemplate
    {
        public int ID { get; }
        public int[] PackageSN { get; }

        public CashPackageTemplate(int id, IDataProperty property)
        {
            ID = id;
            PackageSN = property.Resolve("SN").Children
                .Select(c => c.Resolve<int>() ?? 0)
                .ToArray();
        }
    }
}