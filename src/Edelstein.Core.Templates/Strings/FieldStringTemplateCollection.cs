using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Edelstein.Provider;
using MoreLinq.Extensions;

namespace Edelstein.Core.Templates.Strings
{
    public class FieldStringTemplateCollection : AbstractEagerDataTemplateCollection
    {
        private readonly IDataDirectoryCollection _collection;

        public FieldStringTemplateCollection(IDataDirectoryCollection collection)
            => _collection = collection;

        protected override async Task<IEnumerable<IDataTemplate>> Load()
        {
            var property = _collection.Resolve("String/Map.img");

            return property.Children
                .SelectMany(c => c.Children)
                .DistinctBy(c => c.Name)
                .Where(c => c.Name.All(char.IsDigit))
                .Select(c => new FieldStringTemplate(Convert.ToInt32(c.Name), c.ResolveAll()));
        }
    }
}