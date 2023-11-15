using System.Collections.Frozen;
using Edelstein.Protocol.Utilities.Templates;

namespace Edelstein.Common.Utilities.Templates;

public class TemplateManager<TTemplate> : ITemplateManager<TTemplate> where TTemplate : ITemplate
{
    private readonly IDictionary<int, ITemplateProvider<TTemplate>> _providers;
    private FrozenDictionary<int, ITemplateProvider<TTemplate>>? _providersFrozen;

    public TemplateManager() => _providers = new Dictionary<int, ITemplateProvider<TTemplate>>();

    public int Count => (_providersFrozen ?? _providers).Count;

    public async Task<TTemplate?> Retrieve(int key) =>
        (_providersFrozen ?? _providers).TryGetValue(key, out var provider) ? await provider.Provide() : default;

    public Task<ITemplateProvider<TTemplate>> Insert(ITemplateProvider<TTemplate> entry) =>
        Task.FromResult((_providersFrozen ?? _providers)[entry.ID] = entry);

    public async Task<ICollection<TTemplate>> RetrieveAll() =>
        await Task.WhenAll((_providersFrozen ?? _providers).Values.Select(p => p.Provide()));

    public void Freeze()
    {
        _providersFrozen = _providers.ToFrozenDictionary();
        _providers.Clear();
    }
}
