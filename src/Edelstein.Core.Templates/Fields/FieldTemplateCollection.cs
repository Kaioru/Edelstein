using System.Threading.Tasks;
using Edelstein.Core.Provider;

namespace Edelstein.Core.Templates.Fields
{
    public class FieldTemplateCollection : AbstractLazyDataTemplateCollection
    {
        private readonly IDataDirectoryCollection _collection;

        public FieldTemplateCollection(IDataDirectoryCollection collection)
            => _collection = collection;

        public override async Task<IDataTemplate> Load(int id)
        {
            var property = _collection.Resolve($"Map/Map/Map{id.ToString("D9")[0]}/{id:D9}.img");
            var info = property.Resolve("info");
            var link = info.Resolve<int>("link");

            if (link.HasValue) return await GetAsync(link.Value);

            return new FieldTemplate(id, property.ResolveAll());
        }
    }
}