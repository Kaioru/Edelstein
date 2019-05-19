namespace Edelstein.Provider.Templates.Quest
{
    public class QuestTemplateCollection : AbstractLazyTemplateCollection
    {
        public override TemplateCollectionType Type { get; }

        public QuestTemplateCollection(IDataDirectoryCollection collection) : base(collection)
        {
        }

        public override ITemplate Load(int id)
        {
            var info = Collection.Resolve($"Quest/QuestInfo.img/{id}");
            var act = Collection.Resolve($"Quest/Act.img/{id}");
            var check = Collection.Resolve($"Quest/Check.img/{id}");

            return new QuestTemplate(
                id,
                info.ResolveAll(),
                act.ResolveAll(),
                check.ResolveAll()
            );
        }
    }
}