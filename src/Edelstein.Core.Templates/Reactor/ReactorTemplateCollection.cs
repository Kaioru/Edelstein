using System.Threading.Tasks;
using Edelstein.Core.Provider;

namespace Edelstein.Core.Templates.Reactor
{
    public class ReactorTemplateCollection : AbstractLazyDataTemplateCollection
    {
        private readonly IDataDirectoryCollection _collection;

        public ReactorTemplateCollection(IDataDirectoryCollection collection)
            => _collection = collection;

        public override async Task<IDataTemplate> Load(int id)
        {
            var property = _collection.Resolve($"Reactor/{id:D7}.img");
            return new ReactorTemplate(id, property);
        }
    }
}