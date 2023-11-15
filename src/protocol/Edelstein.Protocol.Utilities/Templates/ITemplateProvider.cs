using Edelstein.Protocol.Utilities.Repositories;

namespace Edelstein.Protocol.Utilities.Templates;

public interface ITemplateProvider<TTemplate> : 
    IIdentifiable<int> 
    where TTemplate : ITemplate
{
    Task<TTemplate> Provide();
}
