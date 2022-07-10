using System.Collections.Generic;
using System.Threading.Tasks;
using Edelstein.Protocol.Parser;
using reWZ;

namespace Edelstein.Common.Parser.reWZ
{
    public class WZDataDirectory : IDataDirectory
    {
        private readonly WZFile _file;

        public string Name => _file.MainDirectory.Name;
        public string Path => _file.MainDirectory.Path;

        public IEnumerable<IDataProperty> Children
        {
            get
            {
                foreach (var child in _file.MainDirectory)
                    yield return new WZDataProperty(child);
            }
        }

        public WZDataDirectory(WZFile file)
        {
            _file = file;
        }

        public IDataProperty Resolve(string path = null)
            => new WZDataProperty(_file.MainDirectory).Resolve(path);

        public Task<IDataProperty> ResolveAsync(string path = null)
            => Task.Run(() => Resolve(path));
    }
}