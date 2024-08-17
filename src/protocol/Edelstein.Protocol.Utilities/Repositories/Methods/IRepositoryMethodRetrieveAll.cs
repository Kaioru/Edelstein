using System.Collections.Generic;
using System.Threading.Tasks;

namespace Edelstein.Protocol.Utilities.Repositories.Methods;

public interface IRepositoryMethodRetrieveAll<in TKey, TEntry>
    where TKey : notnull
    where TEntry : IRepositoryEntry<TKey>
{
    Task<ICollection<TEntry>> RetrieveAll();
}
