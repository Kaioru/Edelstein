using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using PKG1;

namespace Edelstein.Provider.Parser.WZ
{
    public class WZDataDirectoryCollection : IDataDirectoryCollection
    {
        private readonly PackageCollection _collection;

        public IDataProperty Parent => null;

        public IEnumerable<IDataDirectory> Children => _collection.Packages.Values
            .Select(p => new WZDataDirectory(p));

        public WZDataDirectoryCollection(PackageCollection collection)
        {
            _collection = collection;
            WZReader.InitializeKeys();
        }

        public WZDataDirectoryCollection(string path) : this(new PackageCollection(Path.Combine(path, "Base.wz")))
        {
        }

        public IDataProperty Resolve(string path = null)
            => new WZDataProperty(_collection.Resolve(path));

        public Task<IDataProperty> ResolveAsync(string path = null)
            => Task.Run(() => Resolve(path));
    }
}