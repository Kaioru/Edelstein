using System.Threading.Tasks;
using Edelstein.Protocol.Utilities.Repositories;

namespace Edelstein.Protocol.Utilities.Templates;

public interface ITemplateCollectionProvider<TTemplate> : 
    IRepositoryEntry<int> 
    where TTemplate : ITemplate
{
    Task<TTemplate> Provide();
}
