using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using Edelstein.Provider.Parser;
using MoreLinq.Extensions;

namespace Edelstein.Provider.Templates.Field
{
    public class FieldTemplateCollection : AbstractLazyTemplateCollection
    {
        public FieldTemplateCollection(IDataDirectoryCollection collection) : base(collection)
        {
        }

        public override Task<ITemplate> Load(int id)
        {
            var property = Collection.Resolve($"Map/Map/Map{id.ToString("D8")[0]}/{id:D8}.img");

            property = property ?? Collection.Resolve($"Map/Map/Map{id.ToString("D9")[0]}/{id:D9}.img");

            var info = property.Resolve("info");
            var link = info.Resolve<int>("link");
            if (link.HasValue) Load(link.Value);

            return Task.FromResult<ITemplate>(FieldTemplate.Parse(id, property));
        }
    }
}