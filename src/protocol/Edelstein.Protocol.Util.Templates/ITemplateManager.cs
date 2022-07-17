using Edelstein.Protocol.Util.Repositories.Methods;

namespace Edelstein.Protocol.Util.Templates;

public interface ITemplateManager<TTemplate> :
    IRepositoryMethodRetrieve<int, TTemplate>,
    IRepositoryMethodRetrieveAll<int, TTemplate>,
    IRepositoryMethodInsert<int, ITemplateProvider<TTemplate>>
    where TTemplate : ITemplate
{
    int Count { get; }
}
