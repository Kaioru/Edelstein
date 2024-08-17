using Edelstein.Protocol.Utilities.Repositories.Methods;

namespace Edelstein.Protocol.Utilities.Templates;

public interface ITemplateManagerContext<TTemplate> :
    IRepositoryMethodInsert<int, ITemplateProvider<TTemplate>>,
    IRepositoryMethodRetrieve<int, ITemplateProvider<TTemplate>>,
    IRepositoryMethodRetrieveAll<int, ITemplateProvider<TTemplate>>
    where TTemplate : ITemplate
{
    int Count { get; }
}
