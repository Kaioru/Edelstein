using Edelstein.Provider.Parsing;

namespace Edelstein.Provider.Templates.Field.Reactor
{
    public class ReactorTemplateCollection : AbstractLazyTemplateCollection
    {
        public override TemplateCollectionType Type => TemplateCollectionType.Reactor;

        public ReactorTemplateCollection(IDataDirectoryCollection collection) : base(collection)
        {
        }

        public override ITemplate Load(int id)
        {
            var property = Collection.Resolve($"Reactor/{id:D7}.img");

            return new ReactorTemplate(id, property);
        }
    }
}