using System;

namespace Edelstein.Core.Database
{
    public interface IDataSession : IDataAction, IDisposable
    {
        IDataQuery<T> Query<T>() where T : class, IDataEntity;
        IDataBatch Batch();
    }
}