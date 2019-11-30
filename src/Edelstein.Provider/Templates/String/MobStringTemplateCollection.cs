using System;
using System.Collections.Generic;
using System.Linq;
using Edelstein.Provider.Parsing;
using MoreLinq;

namespace Edelstein.Provider.Templates.String
{
    public class MobStringTemplateCollection : AbstractEagerTemplateCollection
    {
        public override TemplateCollectionType Type => TemplateCollectionType.MobString;

        public MobStringTemplateCollection(IDataDirectoryCollection collection) : base(collection)
        {
        }

        protected override IEnumerable<ITemplate> Load()
        {
            var templates = new List<ITemplate>();
            var property = Collection.Resolve("String");

            property.ResolveAll(p =>
            {
                p.Resolve("Mob.img").Children
                    .DistinctBy(c => c.Name)
                    .Where(c => c.Name.All(char.IsDigit))
                    .Select(c => new MobStringTemplate(
                        Convert.ToInt32(c.Name),
                        c.ResolveAll()))
                    .ForEach(templates.Add);
            });

            return templates;
        }
    }
}