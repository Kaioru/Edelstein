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
        private readonly IDictionary<Type, IDataTemplateCollection> _collections;

        public DataTemplateManager()
            => _collections = new ConcurrentDictionary<Type, IDataTemplateCollection>();

        public async Task Register<T>(IDataTemplateCollection collection) where T : IDataTemplate
        {
            _collections[typeof(T)] = collection;

            if (collection is AbstractEagerDataTemplateCollection eagerCollection)
            {
                var watch = Stopwatch.StartNew();

                await eagerCollection.Populate();
                Logger.Info(
                    $"Loaded {eagerCollection.GetAll().Count()} {typeof(T).Name} in {watch.ElapsedMilliseconds}ms"
                );
            }
        }

        public Task Deregister<T>() where T : IDataTemplate
        {
            _collections.Remove(typeof(T));
            return Task.CompletedTask;
        }

        public T Get<T>(int id) where T : IDataTemplate
            => _collections.ContainsKey(typeof(T))
                ? (T) _collections[typeof(T)].Get(id)
                : default;

        public IEnumerable<T> GetAll<T>() where T : IDataTemplate
            => _collections.ContainsKey(typeof(T))
                ? _collections[typeof(T)].GetAll().OfType<T>()
                : new List<T>();

        public async Task<T> GetAsync<T>(int id) where T : IDataTemplate
            => _collections.ContainsKey(typeof(T))
                ? (T) await _collections[typeof(T)].GetAsync(id)
                : default;

        public async Task<IEnumerable<T>> GetAllAsync<T>() where T : IDataTemplate
            => _collections.ContainsKey(typeof(T))
                ? (await _collections[typeof(T)].GetAllAsync()).OfType<T>()
                : new List<T>();
    }
}