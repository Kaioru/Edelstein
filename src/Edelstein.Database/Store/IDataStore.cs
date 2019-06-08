using System;

namespace Edelstein.Database.Store
{
    public interface IDataStore : IDisposable
    {
        IDataSession OpenSession();
    }
}