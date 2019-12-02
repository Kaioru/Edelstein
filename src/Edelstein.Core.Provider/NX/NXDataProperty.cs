using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Duey;

namespace Edelstein.Provider.NX
{
    public class NXDataProperty : IDataProperty
    {
        private readonly INXNode _node;

        public string Name => _node.Name;
        public string Path => _node.Name;
        public IDataProperty Parent => new NXDataProperty(_node.Parent);
        public IEnumerable<IDataProperty> Children => _node.Children.Select(c => new NXDataProperty(c));

        public NXDataProperty(INXNode node)
            => _node = node;

        public IDataProperty Resolve(string path = null)
        {
            var node = _node.ResolvePath(path);
            return node == null ? null : new NXDataProperty(node);
        }

        public IDataProperty ResolveAll()
            => new NXDataProperty(_node.ResolveAll());

        public void ResolveAll(Action<IDataProperty> context)
            => context.Invoke(ResolveAll());

        public T? Resolve<T>(string path = null) where T : struct
            => _node.Resolve<T>(path);

        public T ResolveOrDefault<T>(string path = null) where T : class
            => _node.ResolveOrDefault<T>(path);

        public Task<IDataProperty> ResolveAsync(string path = null)
            => Task.Run(() => Resolve(path));

        public Task<T?> ResolveAsync<T>(string path = null) where T : struct
            => Task.Run(() => Resolve<T>(path));

        public Task<T> ResolveOrDefaultAsync<T>(string path = null) where T : class
            => Task.Run(() => ResolveOrDefaultAsync<T>(path));
    }
}