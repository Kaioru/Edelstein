using System.Collections.Generic;
using System.Threading.Tasks;

namespace Edelstein.Protocol.Parser
{
    public interface IDataDirectoryCollection
    {
        IEnumerable<IDataDirectory> Children { get; }

        IDataProperty Resolve(string path = null);
        Task<IDataProperty> ResolveAsync(string path = null);
    }
}
