using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Edelstein.Provider;

namespace Edelstein.Core.Templates.Server.Shop
{
    public class NotSaleTemplateCollection : AbstractEagerDataTemplateCollection
    {
        private readonly IDataDirectoryCollection _collection;

        public NotSaleTemplateCollection(IDataDirectoryCollection collection)
            => _collection = collection;

        protected override async Task<IEnumerable<IDataTemplate>> Load()
        {
            var property = _collection.Resolve("Server/NotSale.img");

            return property.Children
                .Select(c => new NotSaleTemplate(
                    c.Resolve<int>() ?? 0
                ));
        }
    }
}