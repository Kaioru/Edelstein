using Edelstein.Protocol.Utilities.Templates;

namespace Edelstein.Common.Utilities.Templates;

public class TemplateManager<TTemplate> : ITemplateManager<TTemplate> where TTemplate : ITemplate
{
    private readonly IDictionary<int, ITemplateProvider<TTemplate>> _providers;

    public TemplateManager() => _providers = new Dictionary<int, ITemplateProvider<TTemplate>>();

    public int Count => _providers.Count;

    public async Task<TTemplate?> Retrieve(int key) =>
        _providers.TryGetValue(key, out var provider) ? await provider.Provide() : default;
    
    public Task<ITemplateProvider<TTemplate>> Insert(ITemplateProvider<TTemplate> entry) =>
        Task.FromResult(_providers[entry.ID] = entry);
    
    public async Task<ICollection<TTemplate>> RetrieveAll() =>
        await Task.WhenAll(_providers.Values.Select(p => p.Provide()));
}
