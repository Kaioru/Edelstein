using System;
using System.Linq;
using System.Threading.Tasks;
using Edelstein.Provider.Parser;
using MoreLinq.Extensions;

namespace Edelstein.Provider.Templates.String
{
    public class FieldStringTemplateCollection : AbstractEagerTemplateCollection
    {
        public FieldStringTemplateCollection(IDataDirectoryCollection collection) : base(collection)
        {
        }

        public override Task LoadAll()
        {
            var property = Collection.Resolve("String/Map.img");

            property.Children
                .SelectMany(c => c.Children)
                .DistinctBy(c => c.Name)
                .Where(c => c.Name.All(char.IsDigit))
                .ToDictionary(
                    c => Convert.ToInt32(c.Name),
                    c => FieldStringTemplate.Parse(Convert.ToInt32(c.Name), c)
                )
                .ForEach(kv => Templates.Add(kv.Key, kv.Value));
            return Task.CompletedTask;
        }
    }
}