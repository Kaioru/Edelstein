using System;
using System.Collections.Generic;
using System.Linq;
using Edelstein.Provider.Parsing;

namespace Edelstein.Provider.Templates.Etc.NPCShop
{
    public class NPCShopTemplateCollection : AbstractEagerTemplateCollection
    {
        public override TemplateCollectionType Type => TemplateCollectionType.NPCShop;

        public NPCShopTemplateCollection(IDataDirectoryCollection collection) : base(collection)
        {
        }

        protected override IEnumerable<ITemplate> Load()
        {
            var property = Collection.Resolve("Server/NpcShop.img");

            return property.Children
                .Select(c => new NPCShopTemplate(Convert.ToInt32(c.Name), c.ResolveAll()));
        }
    }
}