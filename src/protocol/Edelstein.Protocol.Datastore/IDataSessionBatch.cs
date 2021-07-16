using System;
using System.Threading.Tasks;

namespace Edelstein.Protocol.Datastore
{
    public interface IDataSessionBatch : IDataSession, IDisposable
    {
        Task Discard();
        Task Save();
    }
}
