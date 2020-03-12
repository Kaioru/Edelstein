using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Edelstein.Provider;

namespace Edelstein.Core.Templates.Server.Continent
{
    public class ContinentTemplateCollection : AbstractEagerDataTemplateCollection
    {
        private readonly IDataDirectoryCollection _collection;

        public ContinentTemplateCollection(IDataDirectoryCollection collection)
            => _collection = collection;

        protected override async Task<IEnumerable<IDataTemplate>> Load()
        {
            var property = _collection.Resolve("Server/Continent.img");

            return property.Children
                .Select(
                    c => new ContinentTemplate(Convert.ToInt32(c.Name), c)
                );
        }
    }
}