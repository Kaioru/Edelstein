using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Edelstein.Protocol.Gameplay.Templating;
using Microsoft.Extensions.Caching.Memory;

namespace Edelstein.Common.Gameplay.Templating
{
    public class TemplateRepository<TEntry> : ITemplateRepository<TEntry> where TEntry : class, ITemplate
    {
        private readonly IMemoryCache _cache;
        private readonly IDictionary<int, TemplateProvider<TEntry>> _providers;
        private readonly TimeSpan _duration;

        public int Count => _providers.Count;

        public TemplateRepository(TimeSpan duration)
        {
            _cache = new MemoryCache(new MemoryCacheOptions());
            _providers = new Dictionary<int, TemplateProvider<TEntry>>();
            _duration = duration;
        }

        public void Register(TemplateProvider<TEntry> provider)
            => _providers[provider.ID] = provider;

        public async Task<TEntry> Retrieve(int key)
        {
            var entry = _cache.Get<TEntry>(key);

            if (entry == null && _providers.ContainsKey(key))
            {
                var provider = _providers[key];
                entry = await provider.Provide();
            }
            if (entry != null) _cache.Set(key, entry, _duration);
            return entry;
        }

        public async Task<IEnumerable<TEntry>> Retrieve(IEnumerable<int> keys)
            => await Task.WhenAll(keys.Select(key => Retrieve(key)));

        public async Task<IEnumerable<TEntry>> RetrieveAll()
            => await Task.WhenAll(_providers.Keys.Select(key => Retrieve(key)));
    }
}
