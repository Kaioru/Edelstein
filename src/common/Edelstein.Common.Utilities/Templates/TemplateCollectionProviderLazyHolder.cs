using Edelstein.Protocol.Utilities.Templates;

namespace Edelstein.Common.Utilities.Templates;

public record TemplateCollectionProviderLazyHolder<TTemplate>(
    TTemplate Template
) where TTemplate : ITemplate;
