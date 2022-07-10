using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Edelstein.Protocol.Parser;
using reWZ;

namespace Edelstein.Common.Parser.reWZ
{
    public class WZDataDirectoryCollection : IDataDirectoryCollection
    {
        private readonly IDictionary<string, WZFile> _files;

        public IEnumerable<IDataDirectory> Children => _files.Values.Select(f => new WZDataDirectory(f));

        public WZDataDirectoryCollection(string path, WZVariant variant, bool encrypted)
        {
            _files = Directory.GetFiles(path, "*.nx").ToImmutableDictionary(
                Path.GetFileNameWithoutExtension,
                d => new WZFile(d, variant, encrypted)
            );
        }

        public IDataProperty Resolve(string path = null)
        {
            if (string.IsNullOrEmpty(path)) return null;

            var split = path.Split('/');
            return new WZDataProperty(_files[split[0]]?.ResolvePath(string.Join("/", split.Skip(1).ToArray())));
        }

        public Task<IDataProperty> ResolveAsync(string path = null)
            => Task.Run(() => Resolve(path));
    }
}