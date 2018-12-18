using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Edelstein.Provider.Logging;
using Edelstein.Provider.Parser;
using Edelstein.Provider.Templates.Field;
using Edelstein.Provider.Templates.Item;
using Edelstein.Provider.Templates.Item.Option;
using Edelstein.Provider.Templates.Item.Set;

namespace Edelstein.Provider.Templates
{
    public class TemplateManager : ITemplateManager
    {
        private static readonly ILog Logger = LogProvider.GetCurrentClassLogger();

        private readonly IDictionary<Type, ITemplateCollection> _dictionary;

        public TemplateManager(IDataDirectoryCollection collection)
        {
            _dictionary = new Dictionary<Type, ITemplateCollection>
            {
                [typeof(ItemOptionTemplate)] = new ItemOptionTemplateCollection(collection),
                [typeof(SetItemInfoTemplate)] = new SetItemInfoTemplateCollection(collection),
                [typeof(ItemTemplate)] = new ItemTemplateCollection(collection),
                [typeof(FieldTemplate)] = new FieldTemplateCollection(collection)
            };
        }

        public T Get<T>(int id)
            => (T) _dictionary[typeof(T)].Get(id);

        public Task<T> GetAsync<T>(int id)
            => Task.Run(() => Get<T>(id));

        public async Task Load()
        {
            Logger.Info("Loading templates..");
            await Task.WhenAll(_dictionary.Values
                .OfType<AbstractEagerTemplateCollection>()
                .Select(async c =>
                {
                    await c.LoadAll();
                    Logger.Info($"Loaded {c.GetType().Name} with {c.Cache.Count()} templates");
                }));
            Logger.Info("Finished loading templates..");
        }
    }
}