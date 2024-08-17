namespace Edelstein.Protocol.Utilities.Templates;

public interface ITemplateManager<TTemplate> : ITemplateCollection<TTemplate>
    where TTemplate : ITemplate;
