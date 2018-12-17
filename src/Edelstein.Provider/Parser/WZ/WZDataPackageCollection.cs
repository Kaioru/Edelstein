using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PKG1;

namespace Edelstein.Provider.Parser.WZ
{
    public class WZDataPackageCollection : IDataDirectoryCollection
    {
        private readonly PackageCollection _collection;

        public IDataProperty Parent => null;

        public IEnumerable<IDataDirectory> Children => _collection.Packages.Values
            .Select(p => new WZDataPackage(p));

        public WZDataPackageCollection(PackageCollection collection)
        {
            _collection = collection;
            WZReader.InitializeKeys();
        }

        public WZDataPackageCollection(string path) : this(new PackageCollection(path))
        {
        }

        public IDataProperty Resolve(string path = null)
            => new WZDataProperty(_collection.Resolve(path));

        public Task<IDataProperty> ResolveAsync(string path = null)
            => Task.Run(() => Resolve(path));
    }
}