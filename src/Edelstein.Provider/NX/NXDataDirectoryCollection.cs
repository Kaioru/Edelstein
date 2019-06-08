using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Duey.NX;

namespace Edelstein.Provider.NX
{
    public class NXDataDirectoryCollection : IDataDirectoryCollection
    {
        private readonly IDictionary<string, NXFile> _files;

        public IEnumerable<IDataDirectory> Children => _files.Values.Select(f => new NXDataDirectory(f));

        public NXDataDirectoryCollection(string path)
        {
            _files = Directory.GetFiles(path, "*.nx").ToImmutableDictionary(
                Path.GetFileNameWithoutExtension,
                d => new NXFile(d)
            );
        }

        public IDataProperty Resolve(string path = null)
        {
            if (string.IsNullOrEmpty(path)) return null;

            var split = path.Split('/');
            return new NXDataProperty(_files[split[0]]?.ResolvePath(string.Join("/", split.Skip(1).ToArray())));
        }

        public Task<IDataProperty> ResolveAsync(string path = null)
            => Task.Run(() => Resolve(path));
    }
}