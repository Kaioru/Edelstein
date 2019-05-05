using System;
using System.Collections.Generic;
using System.Linq;

namespace Edelstein.Provider.Templates.Field.Continent
{
    public class ContinentTemplateCollection : AbstractEagerTemplateCollection
    {
        public override TemplateCollectionType Type { get; }

        public ContinentTemplateCollection(IDataDirectoryCollection collection) : base(collection)
        {
        }

        protected override IEnumerable<ITemplate> Load()
        {
            var property = Collection.Resolve("Server/Continent.img");

            return property.Children
                .Select(
                    c => new ContinentTemplate(Convert.ToInt32(c.Name), c)
                );
        }
    }
}