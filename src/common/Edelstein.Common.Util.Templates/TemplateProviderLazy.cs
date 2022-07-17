using Edelstein.Protocol.Util.Templates;

namespace Edelstein.Common.Util.Templates;

public class TemplateProviderLazy<TTemplate> : ITemplateProvider<TTemplate> where TTemplate : ITemplate
{
    private readonly Func<TTemplate> _func;

    public TemplateProviderLazy(int id, Func<TTemplate> func)
    {
        ID = id;
        _func = func;
    }

    private TTemplate? Template { get; set; }
    public int ID { get; }

    public Task<TTemplate> Provide() => Task.FromResult(Template ??= _func.Invoke());
}
