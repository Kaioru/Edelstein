using System.Threading.Tasks;
using Edelstein.Provider;

namespace Edelstein.Core.Templates.Mob
{
    public class MobTemplateCollection : AbstractLazyDataTemplateCollection
    {
        private readonly IDataDirectoryCollection _collection;

        public MobTemplateCollection(IDataDirectoryCollection collection)
            => _collection = collection;

        public override async Task<IDataTemplate> Load(int id)
        {
            var property = _collection.Resolve($"Mob/{id:D7}.img");
            return new MobTemplate(id, property);
        }
    }
}