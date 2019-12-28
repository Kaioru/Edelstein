using System;
using System.Threading.Tasks;

namespace Edelstein.Database
{
    public interface IDataBatch : IDataAction, IDisposable
    {
        void SaveChanges();
        Task SaveChangesAsync();
    }
}