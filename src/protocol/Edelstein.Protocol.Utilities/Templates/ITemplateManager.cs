using Edelstein.Protocol.Utilities.Repositories.Methods;

namespace Edelstein.Protocol.Utilities.Templates;

public interface ITemplateManager<TTemplate> :
    ITemplateCollection<TTemplate>,
    IRepositoryMethodInsert<int, ITemplateProvider<TTemplate>>,
    IRepositoryMethodFreeze
    where TTemplate : ITemplate;
