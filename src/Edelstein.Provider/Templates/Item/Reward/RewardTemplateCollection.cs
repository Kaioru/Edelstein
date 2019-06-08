namespace Edelstein.Provider.Templates.Item.Reward
{
    public class RewardTemplateCollection : AbstractLazyTemplateCollection
    {
        public override TemplateCollectionType Type => TemplateCollectionType.Reward;

        public RewardTemplateCollection(IDataDirectoryCollection collection) : base(collection)
        {
        }

        public override ITemplate Load(int id)
        {
            var property = Collection.Resolve($"Server/Reward.img/{id:D7}");

            return new RewardTemplate(id, property.ResolveAll());
        }
    }
}