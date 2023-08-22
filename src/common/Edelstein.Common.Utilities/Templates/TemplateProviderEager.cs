using Edelstein.Protocol.Utilities.Templates;

namespace Edelstein.Common.Utilities.Templates;

public class TemplateProviderEager<TTemplate> : ITemplateProvider<TTemplate> where TTemplate : ITemplate
{
    private readonly TTemplate _template;

    public TemplateProviderEager(int id, TTemplate template)
    {
        ID = id;
        _template = template;
    }

    public int ID { get; }

    public Task<TTemplate> Provide() => Task.FromResult(_template);
}
