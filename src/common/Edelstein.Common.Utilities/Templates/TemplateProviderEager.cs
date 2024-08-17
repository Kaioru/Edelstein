using System.Threading.Tasks;
using Edelstein.Protocol.Utilities.Templates;

namespace Edelstein.Common.Utilities.Templates;

public class TemplateProviderEager<TTemplate>(
    int id, 
    TTemplate template
) : ITemplateProvider<TTemplate>
    where TTemplate : ITemplate
{
    public int ID { get; } = id;

    public Task<TTemplate> Provide() => Task.FromResult(template);
}
