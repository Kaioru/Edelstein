using Edelstein.Protocol.Util.Repositories;

namespace Edelstein.Protocol.Util.Templates;

public interface ITemplateProvider<TTemplate> : IIdentifiable<int> where TTemplate : ITemplate
{
    Task<TTemplate> Provide();
}
