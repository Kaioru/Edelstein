using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Edelstein.Core.Provider;

namespace Edelstein.Core.Templates.Items.ItemOption
{
    public class ItemOptionTemplateCollection : AbstractEagerDataTemplateCollection
    {
        private readonly IDataDirectoryCollection _collection;

        public ItemOptionTemplateCollection(IDataDirectoryCollection collection)
            => _collection = collection;

        protected override async Task<IEnumerable<IDataTemplate>> Load()
        {
            var property = _collection.Resolve("Item/ItemOption.img");

            return property.Children
                .Select(p => new ItemOptionTemplate(
                    Convert.ToInt32(p.Name),
                    p.ResolveAll()
                ));
        }
    }
}