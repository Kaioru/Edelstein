using System;
using System.Collections.Generic;
using System.Linq;

namespace Edelstein.Provider.Templates.Etc
{
    public class CashPackageTemplateCollection : AbstractEagerTemplateCollection
    {
        public override TemplateCollectionType Type { get; }

        public CashPackageTemplateCollection(IDataDirectoryCollection collection) : base(collection)
        {
        }

        protected override IEnumerable<ITemplate> Load()
        {
            var property = Collection.Resolve("Etc/CashPackage.img");

            return property.Children
                .Select(c => new CashPackageTemplate(
                    Convert.ToInt32(c.Name),
                    c.ResolveAll()
                ));
        }
    }
}