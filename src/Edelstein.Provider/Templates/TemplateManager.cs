using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Edelstein.Provider.Parser;

namespace Edelstein.Provider.Templates
{
    public class TemplateManager : ITemplateManager
    {
        private IDataDirectoryCollection _collection;
        private IDictionary<Type, ITemplateCollection> _dictionary;

        public TemplateManager(IDataDirectoryCollection collection)
        {
            _collection = collection;
            _dictionary = new Dictionary<Type, ITemplateCollection>();
        }

        public T Get<T>(int id)
            => (T) _dictionary[typeof(T)].Get(id);

        public Task<T> GetAsync<T>(int id)
            => Task.Run(() => Get<T>(id));

        public Task Load()
            => Task.WhenAll(_dictionary.Values
                .OfType<AbstractEagerTemplateCollection>()
                .Select(c => c.LoadAll()));
    }
}