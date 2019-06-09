using System;
using System.Collections.Generic;
using System.Linq;
using Edelstein.Provider.Parsing;
using MoreLinq;

namespace Edelstein.Provider.Templates.String
{
    public class FieldStringTemplateCollection : AbstractEagerTemplateCollection
    {
        public override TemplateCollectionType Type => TemplateCollectionType.FieldString;

        public FieldStringTemplateCollection(IDataDirectoryCollection collection) : base(collection)
        {
        }

        protected override IEnumerable<ITemplate> Load()
        {
            var property = Collection.Resolve("String/Map.img");

            return property.Children
                .SelectMany(c => c.Children)
                .DistinctBy(c => c.Name)
                .Where(c => c.Name.All(char.IsDigit))
                .Select(c => new FieldStringTemplate(Convert.ToInt32(c.Name), c.ResolveAll()));
        }
    }
}