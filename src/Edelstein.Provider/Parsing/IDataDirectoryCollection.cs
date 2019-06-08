using System.Collections.Generic;
using System.Threading.Tasks;

namespace Edelstein.Provider.Parsing
{
    public interface IDataDirectoryCollection
    {
        IEnumerable<IDataDirectory> Children { get; }

        IDataProperty Resolve(string path = null);
        Task<IDataProperty> ResolveAsync(string path = null);
    }
}