using System;
using System.Threading.Tasks;

namespace Edelstein.Core.Database
{
    public interface IDataBatch : IDataAction, IDisposable
    {
        void SaveChanges();
        Task SaveChangesAsync();
    }
}