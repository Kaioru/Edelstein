using Edelstein.Protocol.Utilities.Repositories.Methods;

namespace Edelstein.Protocol.Utilities.Templates;

public interface ITemplateCollection<TTemplate> :
    IRepositoryMethodRetrieve<int, TTemplate>,
    IRepositoryMethodRetrieveAll<int, TTemplate>
    where TTemplate : ITemplate
{
    int Count { get; }
}
