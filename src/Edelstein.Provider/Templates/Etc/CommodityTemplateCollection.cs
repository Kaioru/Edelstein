using System.Collections.Generic;
using System.Linq;

namespace Edelstein.Provider.Templates.Etc
{
    public class CommodityTemplateCollection : AbstractEagerTemplateCollection
    {
        public override TemplateCollectionType Type => TemplateCollectionType.Commodity;

        public CommodityTemplateCollection(IDataDirectoryCollection collection) : base(collection)
        {
        }

        protected override IEnumerable<ITemplate> Load()
        {
            var property = Collection.Resolve("Etc/Commodity.img");

            return property.Children
                .Select(c => new CommodityTemplate(
                    c.Resolve<int>("SN") ?? 0,
                    c.ResolveAll()
                ));
        }
    }
}