using System.Collections.Generic;
using System.Linq;

namespace Edelstein.Provider.Templates.Server
{
    public class NotSaleTemplateCollection : AbstractEagerTemplateCollection
    {
        public override TemplateCollectionType Type { get; }

        public NotSaleTemplateCollection(IDataDirectoryCollection collection) : base(collection)
        {
        }

        protected override IEnumerable<ITemplate> Load()
        {
            var property = Collection.Resolve("Server/NotSale.img");

            return property.Children
                .Select(c => new NotSaleTemplate(
                    c.Resolve<int>() ?? 0
                ));
        }
    }
}