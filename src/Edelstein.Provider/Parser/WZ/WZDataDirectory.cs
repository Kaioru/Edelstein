using System.Collections.Generic;
using System.Threading.Tasks;
using PKG1;

namespace Edelstein.Provider.Parser.WZ
{
    public class WZDataDirectory : IDataDirectory
    {
        private readonly Package _package;

        public string Name => _package.FileName;
        public string Path => _package.FilePath;

        public IEnumerable<IDataProperty> Children =>
            new List<IDataProperty> {new WZDataProperty(_package.MainDirectory)};

        public WZDataDirectory(Package package)
            => _package = package;

        public IDataProperty Resolve(string path = null)
            => new WZDataProperty(_package.Resolve(path));

        public Task<IDataProperty> ResolveAsync(string path = null)
            => Task.Run(() => Resolve(path));
    }
}