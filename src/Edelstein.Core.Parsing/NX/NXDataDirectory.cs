using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Duey;

namespace Edelstein.Core.Provider.NX
{
    public class NXDataDirectory : IDataDirectory
    {
        private readonly NXFile _file;

        public string Name => _file.Root.Name;
        public string Path => _file.Root.Name;
        public IEnumerable<IDataProperty> Children => _file.Root.Children.Select(c => new NXDataProperty(c));

        public NXDataDirectory(NXFile file)
            => _file = file;

        public IDataProperty Resolve(string path = null)
            => new NXDataProperty(_file.Root).Resolve(path);

        public Task<IDataProperty> ResolveAsync(string path = null)
            => Task.Run(() => Resolve(path));
    }
}