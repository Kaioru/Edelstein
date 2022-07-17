using Edelstein.Protocol.Util.Templates;

namespace Edelstein.Common.Util.Templates;

public class TemplateManager<TTemplate> : ITemplateManager<TTemplate> where TTemplate : ITemplate
{
    private readonly Dictionary<int, ITemplateProvider<TTemplate>> _providers;

    public TemplateManager() => _providers = new Dictionary<int, ITemplateProvider<TTemplate>>();

    public async Task<TTemplate?> Retrieve(int key) =>
        _providers.ContainsKey(key) ? await _providers[key].Provide() : default;

    public async Task<IEnumerable<TTemplate>> RetrieveAll() =>
        await Task.WhenAll(_providers.Values.Select(p => p.Provide()));

    public int Count => throw new NotImplementedException();

    public Task<ITemplateProvider<TTemplate>> Insert(ITemplateProvider<TTemplate> entry) =>
        Task.FromResult(_providers[entry.ID] = entry);
}
