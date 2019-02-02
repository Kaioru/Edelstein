using System;
using System.Linq;
using System.Threading.Tasks;
using Edelstein.Provider.Parser;
using MoreLinq.Extensions;

namespace Edelstein.Provider.Templates.Etc
{
    public class CashPackageTemplateCollection : AbstractEagerTemplateCollection
    {
        public CashPackageTemplateCollection(IDataDirectoryCollection collection) : base(collection)
        {
        }

        public override Task LoadAll()
        {
            var property = Collection.Resolve("Etc/CashPackage.img");

            property.Children
                .ToDictionary(
                    c => Convert.ToInt32(c.Name),
                    c => CashPackageTemplate.Parse(Convert.ToInt32(c.Name), c)
                )
                .ForEach(kv => Templates.Add(kv.Key, kv.Value));
            return Task.CompletedTask;
        }
    }
}