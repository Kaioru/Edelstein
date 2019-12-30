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
                    $"Loaded {eagerCollection.GetAll().Count()} {type.Name} in {watch.ElapsedMilliseconds}ms"
                );
            }
        }

        public Task Deregister(Type type)
        {
            Collections.Remove(type);
            return Task.CompletedTask;
        }

        public T Get<T>(int id) where T : IDataTemplate
            => Collections.ContainsKey(typeof(T))
                ? (T) Collections[typeof(T)].Get(id)
                : default;

        public IEnumerable<T> GetAll<T>() where T : IDataTemplate
            => Collections.ContainsKey(typeof(T))
                ? Collections[typeof(T)].GetAll().OfType<T>()
                : new List<T>();

        public async Task<T> GetAsync<T>(int id) where T : IDataTemplate
            => Collections.ContainsKey(typeof(T))
                ? (T) await Collections[typeof(T)].GetAsync(id)
                : default;

        public async Task<IEnumerable<T>> GetAllAsync<T>() where T : IDataTemplate
            => Collections.ContainsKey(typeof(T))
                ? (await Collections[typeof(T)].GetAllAsync()).OfType<T>()
                : new List<T>();
    }
}