using System.Linq;
using Edelstein.Provider;

namespace Edelstein.Core.Templates.Server.Shop
{
    public class CashPackageTemplate : IDataTemplate
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