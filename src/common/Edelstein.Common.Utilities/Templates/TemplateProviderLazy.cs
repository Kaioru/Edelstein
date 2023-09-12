using Edelstein.Protocol.Utilities.Templates;

namespace Edelstein.Common.Utilities.Templates;

public class TemplateProviderLazy<TTemplate> : ITemplateProvider<TTemplate> where TTemplate : ITemplate
{
    private readonly Func<TTemplate> _func;
    private TTemplate? _template;

    public TemplateProviderLazy(int id, Func<TTemplate> func)
    {
        ID = id;
        _func = func;
        _template = default;
    }

    public int ID { get; }

    public Task<TTemplate> Provide() 
        => Task.FromResult(_template ??= _func.Invoke());
}
