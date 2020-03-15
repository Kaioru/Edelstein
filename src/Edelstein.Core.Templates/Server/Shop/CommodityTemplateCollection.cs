using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Edelstein.Provider;

namespace Edelstein.Core.Templates.Server.Shop
{
    public class CommodityTemplateCollection : AbstractEagerDataTemplateCollection
    {
        private readonly IDataDirectoryCollection _collection;

        public CommodityTemplateCollection(IDataDirectoryCollection collection)
            => _collection = collection;

        protected override async Task<IEnumerable<IDataTemplate>> Load()
        {
            var property = _collection.Resolve("Etc/Commodity.img");

            return property.Children
                .Select(c => new CommodityTemplate(
                    c.Resolve<int>("SN") ?? 0,
                    c.ResolveAll()
                ));
        }
    }
}