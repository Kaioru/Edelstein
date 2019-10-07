using Edelstein.Provider.Parsing;

namespace Edelstein.Provider.Templates.Field
{
    public class FieldTemplateCollection : AbstractLazyTemplateCollection
    {
        public override TemplateCollectionType Type => TemplateCollectionType.Field;

        public FieldTemplateCollection(IDataDirectoryCollection collection) : base(collection)
        {
        }

        public override ITemplate Load(int id)
        {
            var property = Collection.Resolve($"Map/Map/Map{id.ToString("D9")[0]}/{id:D9}.img");

            var info = property.Resolve("info");
            var link = info.Resolve<int>("link");
            if (link.HasValue) Load(link.Value);

            return new FieldTemplate(id, property.ResolveAll());
        }
    }
}