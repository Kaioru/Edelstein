using System;
using System.Collections.Generic;
using System.Linq;

namespace Edelstein.Provider.Templates.Item.Set
{
    public class SetItemInfoTemplateCollection : AbstractEagerTemplateCollection
    {
        public override TemplateCollectionType Type => TemplateCollectionType.SetItemInfo;

        public SetItemInfoTemplateCollection(IDataDirectoryCollection collection) : base(collection)
        {
        }

        protected override IEnumerable<ITemplate> Load()
        {
            var property = Collection.Resolve("Etc/SetItemInfo.img");

            return property.Children
                .Select(p => new SetItemInfoTemplate(
                    Convert.ToInt32(p.Name),
                    p.ResolveAll()
                ));
        }
    }
}