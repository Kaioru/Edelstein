using System;
using System.Threading.Tasks;

namespace Edelstein.Database.Store
{
    public interface IDataBatch : IDataAction, IDisposable
    {
        void SaveChanges();
        Task SaveChangesAsync();
    }
}