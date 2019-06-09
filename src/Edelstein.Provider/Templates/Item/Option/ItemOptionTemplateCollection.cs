using System;
using System.Collections.Generic;
using System.Linq;
using Edelstein.Provider.Parsing;

namespace Edelstein.Provider.Templates.Item.Option
{
    public class ItemOptionTemplateCollection : AbstractEagerTemplateCollection
    {
        public override TemplateCollectionType Type => TemplateCollectionType.ItemOption;

        public ItemOptionTemplateCollection(IDataDirectoryCollection collection) : base(collection)
        {
        }

        protected override IEnumerable<ITemplate> Load()
        {
            var property = Collection.Resolve("Item/ItemOption.img");

            return property.Children
                .Select(p => new ItemOptionTemplate(
                    Convert.ToInt32(p.Name),
                    p.ResolveAll()
                ));
        }
    }
}