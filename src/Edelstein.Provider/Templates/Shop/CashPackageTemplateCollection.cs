using System;
using System.Collections.Generic;
using System.Linq;
using Edelstein.Provider.Parsing;

namespace Edelstein.Provider.Templates.Shop
{
    public class CashPackageTemplateCollection : AbstractEagerTemplateCollection
    {
        public override TemplateCollectionType Type => TemplateCollectionType.CashPackage;

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