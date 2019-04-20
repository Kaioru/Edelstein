using System.Collections.Generic;

namespace Edelstein.Provider.Templates
{
    public abstract class AbstractEagerTemplateCollection : ITemplateCollection
    {
        public abstract TemplateCollectionType Type { get; }
        public IDataDirectoryCollection Collection { get; }
        private readonly IDictionary<int, ITemplate> _templates;

        public AbstractEagerTemplateCollection(IDataDirectoryCollection collection)
        {
            Collection = collection;
            _templates = new Dictionary<int, ITemplate>();
        }

        public ITemplate Get(int id)
        {
            return _templates[id];
        }

        public IEnumerable<ITemplate> GetAll()
        {
            return _templates.Values;
        }

        public abstract void Load();
    }
}