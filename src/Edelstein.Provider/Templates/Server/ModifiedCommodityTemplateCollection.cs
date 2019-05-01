using System;
using System.Collections.Generic;
using System.Linq;

namespace Edelstein.Provider.Templates.Server
{
    public class ModifiedCommodityTemplateCollection : AbstractEagerTemplateCollection
    {
        public override TemplateCollectionType Type { get; }

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