using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Edelstein.Provider.Logging;

namespace Edelstein.Provider
{
    public class DataTemplateManager : IDataTemplateManager
    {
        private static readonly ILog Logger = LogProvider.GetCurrentClassLogger();
        public IDictionary<Type, IDataTemplateCollection> Collections { get; }

        public DataTemplateManager()
            => Collections = new ConcurrentDictionary<Type, IDataTemplateCollection>();

        public Task Register<T>(IDataTemplateCollection collection) where T : IDataTemplate
            => Register(typeof(T), collection);

        public Task Deregister<T>() where T : IDataTemplate
            => Deregister(typeof(T));

        public async Task Register(Type type, IDataTemplateCollection collection)
        {
            Collections[type] = collection;

            if (collection is AbstractEagerDataTemplateCollection eagerCollection)
            {
                var watch = Stopwatch.StartNew();

                await eagerCollection.Populate();
                Logger.Info(
                    $"Loaded {eagerCollection.GetAll().Count()} " +
                    $"{type.Name.Replace("Template", "")} " +
                    $"templates in {watch.ElapsedMilliseconds}ms"
                );
            }
        }

        public Task Deregister(Type type)
        {
            Collections.Remove(type);
            return Task.CompletedTask;
        }

        public T Get<T>(int id) where T : IDataTemplate
            => Collections.TryGetValue(typeof(T), out var collection)
                ? (T) collection.Get(id)
                : default;

        public IEnumerable<T> GetAll<T>() where T : IDataTemplate
            => Collections.TryGetValue(typeof(T), out var collection)
                ? collection.GetAll().OfType<T>()
                : new List<T>();

        public async Task<T> GetAsync<T>(int id) where T : IDataTemplate
            => Collections.TryGetValue(typeof(T), out var collection)
                ? (T) await collection.GetAsync(id)
                : default;

        public async Task<IEnumerable<T>> GetAllAsync<T>() where T : IDataTemplate
            => Collections.TryGetValue(typeof(T), out var collection)
                ? (await collection.GetAllAsync()).OfType<T>()
                : new List<T>();
    }
}