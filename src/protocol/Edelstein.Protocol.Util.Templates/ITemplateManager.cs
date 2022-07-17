using Edelstein.Protocol.Util.Repositories.Methods;

namespace Edelstein.Protocol.Util.Templates;

public interface ITemplateManager<TTemplate> :
    IRepositoryMethodRetrieve<int, TTemplate>,
    IRepositoryMethodRetrieveAll<int, TTemplate>
    where TTemplate : ITemplate
{
}
