using Edelstein.Protocol.Utilities.Templates;

namespace Edelstein.Common.Utilities.Templates;

public record TemplateProviderLazyHolder<TTemplate>(
    TTemplate Template
) where TTemplate : ITemplate;
