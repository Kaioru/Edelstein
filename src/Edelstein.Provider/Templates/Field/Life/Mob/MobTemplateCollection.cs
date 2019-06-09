using Edelstein.Provider.Parsing;

namespace Edelstein.Provider.Templates.Field.Life.Mob
{
    public class MobTemplateCollection : AbstractLazyTemplateCollection
    {
        public override TemplateCollectionType Type => TemplateCollectionType.Mob;

        public MobTemplateCollection(IDataDirectoryCollection collection) : base(collection)
        {
        }

        public override ITemplate Load(int id)
        {
            var property = Collection.Resolve($"Mob/{id:D7}.img");

            return new MobTemplate(id, property.ResolveAll());
        }
    }
}