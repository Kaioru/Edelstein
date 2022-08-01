using Edelstein.Protocol.Util.Templates;

namespace Edelstein.Common.Util.Templates;

public record TemplateProviderLazyHolder<TTemplate>(
    TTemplate Template
) where TTemplate : ITemplate;
