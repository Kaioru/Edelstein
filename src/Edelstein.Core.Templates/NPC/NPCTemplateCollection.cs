using System.Threading.Tasks;
using Edelstein.Provider;

namespace Edelstein.Core.Templates.NPC
{
    public class NPCTemplateCollection : AbstractLazyDataTemplateCollection
    {
        private readonly IDataDirectoryCollection _collection;

        public NPCTemplateCollection(IDataDirectoryCollection collection)
            => _collection = collection;

        public override async Task<IDataTemplate> Load(int id)
        {
            var property = _collection.Resolve($"Npc/{id:D7}.img");
            return new NPCTemplate(id, property);
        }
    }
}