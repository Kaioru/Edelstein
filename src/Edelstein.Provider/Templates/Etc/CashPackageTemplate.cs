using System.Linq;
using Edelstein.Provider.Parser;

namespace Edelstein.Provider.Templates.Etc
{
    public class CashPackageTemplate: ITemplate
    {
        public int ID { get; set; }
        public int[] PackageSN { get; set; }
        
        public static CashPackageTemplate Parse(int id, IDataProperty property)
        {
            return new CashPackageTemplate
            {
                ID = id,
                PackageSN = property.Resolve("SN").Children
                    .Select(c => c.Resolve<int>() ?? 0)
                    .ToArray()
            };
        }
    }
}