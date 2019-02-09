using System.Linq;
using System.Threading.Tasks;
using Edelstein.Provider.Parser;
using MoreLinq.Extensions;

namespace Edelstein.Provider.Templates.Server.FieldSet
{
    public class FieldSetTemplateCollection: AbstractEagerTemplateCollection
    {
        public FieldSetTemplateCollection(IDataDirectoryCollection collection) : base(collection)
        {
        }

        public override Task LoadAll()
        {
            var property = Collection.Resolve("Server/FieldSet.img");

            var id = 0;
            property.Children
                .ToDictionary(
                    c => id++,
                    c => FieldSetTemplate.Parse(id, c)
                )
                .ForEach(kv => Templates.Add(kv.Key, kv.Value));
            return Task.CompletedTask;
        }
    }
}