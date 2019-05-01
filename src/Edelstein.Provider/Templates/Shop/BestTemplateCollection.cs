using System.Collections.Generic;
using System.Linq;

namespace Edelstein.Provider.Templates.Shop
{
    public class BestTemplateCollection : AbstractEagerTemplateCollection
    {
        public override TemplateCollectionType Type => TemplateCollectionType.Best;

        public BestTemplateCollection(IDataDirectoryCollection collection) : base(collection)
        {
        }

        protected override IEnumerable<ITemplate> Load()
        {
            var property = Collection.Resolve("Server/Best.img");
            var id = 0;
            
            return property.Children
                .SelectMany(c => c.Children)
                .SelectMany(c => c.Children)
                .Select(c => new BestTemplate(
                    ++id,
                    c.ResolveAll()
                ));
        }
    }
}