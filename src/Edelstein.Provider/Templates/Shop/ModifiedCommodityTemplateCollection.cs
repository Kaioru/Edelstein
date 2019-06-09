using System;
using System.Collections.Generic;
using System.Linq;
using Edelstein.Provider.Parsing;

namespace Edelstein.Provider.Templates.Shop
{
    public class ModifiedCommodityTemplateCollection : AbstractEagerTemplateCollection
    {
        public override TemplateCollectionType Type => TemplateCollectionType.ModifiedCommodity;

        public ModifiedCommodityTemplateCollection(IDataDirectoryCollection collection) : base(collection)
        {
        }

        protected override IEnumerable<ITemplate> Load()
        {
            var property = Collection.Resolve("Server/ModifiedCommodity.img");

            return property.Children
                .Select(c => new ModifiedCommodityTemplate(
                    Convert.ToInt32(c.Name),
                    c.ResolveAll()
                ));
        }
    }
}