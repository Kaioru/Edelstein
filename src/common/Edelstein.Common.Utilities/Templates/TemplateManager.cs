using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Edelstein.Protocol.Utilities.Templates;
using Injectio.Attributes;

namespace Edelstein.Common.Utilities.Templates;

[RegisterScoped(ImplementationType = typeof(TemplateManager<>), ServiceType = typeof(ITemplateManager<>))]
public class TemplateManager<TTemplate> : 
    ITemplateManager<TTemplate> 
    where TTemplate : ITemplate
{
    private readonly IDictionary<int, ITemplateCollectionProvider<TTemplate>> _providers = new Dictionary<int, ITemplateCollectionProvider<TTemplate>>();

    public int Count => _providers.Count;
    
    public async Task<TTemplate?> Retrieve(int key) =>
        _providers.TryGetValue(key, out var provider) ? await provider.Provide() : default;

    public async Task<ICollection<TTemplate>> RetrieveAll() =>
        await Task.WhenAll(_providers.Values.Select(p => p.Provide()));

    public Task<ITemplateCollectionProvider<TTemplate>> Insert(ITemplateCollectionProvider<TTemplate> entry) =>
        Task.FromResult(_providers[entry.ID] = entry);
}
