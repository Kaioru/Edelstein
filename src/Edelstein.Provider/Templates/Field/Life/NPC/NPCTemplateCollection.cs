namespace Edelstein.Provider.Templates.Field.Life.NPC
{
    public class NPCTemplateCollection : AbstractLazyTemplateCollection
    {
        public override TemplateCollectionType Type => TemplateCollectionType.NPC;

        public NPCTemplateCollection(IDataDirectoryCollection collection) : base(collection)
        {
        }

        public override ITemplate Load(int id)
        {
            var property = Collection.Resolve($"Npc/{id:D7}.img");
            return new NPCTemplate(id, property);
        }
    }
}