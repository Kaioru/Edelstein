using System;
using System.Threading.Tasks;

namespace Edelstein.Protocol.Parser
{
    public interface IDataProperty
    {
        IDataProperty Parent { get; }

        IDataProperty ResolveAll();
        void ResolveAll(Action<IDataProperty> context);

        T? Resolve<T>(string path = null) where T : struct;
        T ResolveOrDefault<T>(string path = null) where T : class;

        Task<T?> ResolveAsync<T>(string path = null) where T : struct;
        Task<T> ResolveOrDefaultAsync<T>(string path = null) where T : class;
    }
}
