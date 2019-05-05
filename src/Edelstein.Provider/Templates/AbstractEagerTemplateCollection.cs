using System.Collections.Generic;
using System.Linq;
using MoreLinq;

namespace Edelstein.Provider.Templates
{
    public abstract class AbstractEagerTemplateCollection : ITemplateCollection
    {
        public abstract TemplateCollectionType Type { get; }
        public IDataDirectoryCollection Collection { get; }
        public IDictionary<int, ITemplate> Templates { get; }

        public AbstractEagerTemplateCollection(IDataDirectoryCollection collection)
        {
            Collection = collection;
            Templates = new Dictionary<int, ITemplate>();
        }

        public ITemplate Get(int id)
        {
            return !Templates.ContainsKey(id) ? null : Templates[id];
        }

        public IEnumerable<ITemplate> GetAll()
        {
            return Templates.Values;
        }

        public void LoadAll()
        {
            Load()
                .Select(v => new KeyValuePair<int, ITemplate>(v.ID, v))
                .ForEach(kv => Templates.Add(kv));
        }

        protected abstract IEnumerable<ITemplate> Load();
    }
}