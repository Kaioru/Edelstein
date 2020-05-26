using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Edelstein.Core.Provider;

namespace Edelstein.Core.Templates.Server.Shop
{
    public class CashPackageTemplateCollection : AbstractEagerDataTemplateCollection
    {
        private readonly IDataDirectoryCollection _collection;

        public CashPackageTemplateCollection(IDataDirectoryCollection collection)
            => _collection = collection;

        protected override async Task<IEnumerable<IDataTemplate>> Load()
        {
            var property = _collection.Resolve("Etc/CashPackage.img");

            return property.Children
                .Select(c => new CashPackageTemplate(
                    Convert.ToInt32(c.Name),
                    c.ResolveAll()
                ));
        }
    }
}