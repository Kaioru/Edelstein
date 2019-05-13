namespace Edelstein.Provider.Templates.Skill
{
    public class SkillTemplateCollection : AbstractLazyTemplateCollection
    {
        public override TemplateCollectionType Type => TemplateCollectionType.Skill;

        public SkillTemplateCollection(IDataDirectoryCollection collection) : base(collection)
        {
        }

        public override ITemplate Load(int id)
        {
            var property = Collection.Resolve($"Skill/{id / 10000:D3}.img/skill/{id:D7}");
            return new SkillTemplate(id, property);
        }
    }
}