using System;

namespace Edelstein.Database
{
    public interface IDataStore : IDisposable
    {
        IDataSession OpenSession();
    }
}