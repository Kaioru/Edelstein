using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PKG1;

namespace Edelstein.Provider.Parser.WZ
{
    public class WZDataProperty : IDataProperty
    {
        private readonly WZProperty _property;

        public string Name => _property.Name;
        public string Path => _property.Path;
        public IDataProperty Parent => new WZDataProperty(_property.Parent);
        public IEnumerable<IDataProperty> Children => _property.Children.Select(c => new WZDataProperty(c));

        public WZDataProperty(WZProperty property)
            => _property = property;

        public IDataProperty Resolve(string path = null)
            => new WZDataProperty(_property.Resolve(path));

        public T? Resolve<T>(string path = null) where T : struct
            => _property.ResolveFor<T>(path);

        public T ResolveOrDefault<T>(string path = null) where T : class
            => _property.ResolveForOrNull<T>(path);

        public async Task<IDataProperty> ResolveAsync(string path = null)
            => new WZDataProperty(await _property.ResolveAsync(path));

        public Task<T?> ResolveAsync<T>(string path = null) where T : struct
            => _property.ResolveForAsync<T>(path);

        public Task<T> ResolveOrDefaultAsync<T>(string path = null) where T : class
            => _property.ResolveForOrNullAsync<T>(path);
    }
}