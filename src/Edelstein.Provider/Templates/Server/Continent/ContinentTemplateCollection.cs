using System;
using System.Linq;
using System.Threading.Tasks;
using Edelstein.Provider.Parser;
using MoreLinq.Extensions;

namespace Edelstein.Provider.Templates.Server.Continent
{
    public class ContinentTemplateCollection : AbstractEagerTemplateCollection
    {
        public ContinentTemplateCollection(IDataDirectoryCollection collection) : base(collection)
        {
        }

        public override Task LoadAll()
        {
            var property = Collection.Resolve("Server/Continent.img");

            property.Children
                .ToDictionary(
                    c => Convert.ToInt32(c.Name),
                    c => ContinentTemplate.Parse(Convert.ToInt32(c.Name), c)
                )
                .ForEach(kv => Templates.Add(kv.Key, kv.Value));
            return Task.CompletedTask;
        }
    }
}