using Edelstein.Protocol.Utilities.Repositories.Methods;

namespace Edelstein.Protocol.Utilities.Templates;

public interface ITemplateManager<TTemplate> :
    IRepositoryMethodRetrieve<int, TTemplate>,
    IRepositoryMethodRetrieveAll<int, TTemplate>,
    IRepositoryMethodInsert<int, ITemplateProvider<TTemplate>>,
    IRepositoryMethodFreeze
    where TTemplate : ITemplate
{
    int Count { get; }
}
