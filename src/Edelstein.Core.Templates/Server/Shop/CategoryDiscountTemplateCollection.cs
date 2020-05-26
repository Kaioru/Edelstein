using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Edelstein.Core.Provider;

namespace Edelstein.Core.Templates.Server.Shop
{
    public class CategoryDiscountTemplateCollection : AbstractEagerDataTemplateCollection
    {
        private readonly IDataDirectoryCollection _collection;

        public CategoryDiscountTemplateCollection(IDataDirectoryCollection collection)
            => _collection = collection;

        protected override async Task<IEnumerable<IDataTemplate>> Load()
        {
            var property = _collection.Resolve("Server/CategoryDiscount.img");
            var id = 0;

            return property.Children
                .SelectMany(c => c.Children)
                .Select(c => new CategoryDiscountTemplate(
                    ++id,
                    c.ResolveAll()
                ));
        }
    }
}