using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Edelstein.Provider.Parser;
using Edelstein.Provider.Templates.Item;

namespace Edelstein.Provider.Templates
{
    public class TemplateManager : ITemplateManager
    {
        private readonly IDataDirectoryCollection _collection;
        private readonly IDictionary<Type, ITemplateCollection> _dictionary;

        public TemplateManager(IDataDirectoryCollection collection)
        {
            _collection = collection;
            _dictionary = new Dictionary<Type, ITemplateCollection>
            {
                [typeof(ItemTemplate)] = new ItemTemplateManager(_collection)
            };
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