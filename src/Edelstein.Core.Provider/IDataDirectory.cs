using System.Collections.Generic;
using System.Threading.Tasks;

namespace Edelstein.Provider
{
    public interface IDataDirectory
    {
        string Name { get; }
        string Path { get; }
        IEnumerable<IDataProperty> Children { get; }

        IDataProperty Resolve(string path = null);
        Task<IDataProperty> ResolveAsync(string path = null);
    }
}