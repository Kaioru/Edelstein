using System.Threading.Tasks;
using Edelstein.Provider.Parser;

namespace Edelstein.Provider.Templates.NPC
{
    public class NPCTemplateCollection : AbstractLazyTemplateCollection
    {
        public NPCTemplateCollection(IDataDirectoryCollection collection) : base(collection)
        {
        }

        public override Task<ITemplate> Load(int id)
        {
            var property = Collection.Resolve($"Npc/{id:D7}.img");
            return Task.FromResult<ITemplate>(NPCTemplate.Parse(id, property));
        }
    }
}