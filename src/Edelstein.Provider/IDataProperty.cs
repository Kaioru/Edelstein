using System;
using System.Threading.Tasks;

namespace Edelstein.Provider
{
    public interface IDataProperty : IDataDirectory
    {
        IDataProperty Parent { get; }

        void ResolveAll(Action<IDataProperty> context);
        IDataProperty ResolveAll();

        T? Resolve<T>(string path = null) where T : struct;
        T ResolveOrDefault<T>(string path = null) where T : class;

        Task<T?> ResolveAsync<T>(string path = null) where T : struct;
        Task<T> ResolveOrDefaultAsync<T>(string path = null) where T : class;
    }
}