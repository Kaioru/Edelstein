using System;
using Edelstein.Protocol.Util.Repositories;

namespace Edelstein.Protocol.Datastore
{
    public interface IDataDocument : IRepositoryEntry<int>
    {
        DateTime DateCreated { get; }
        DateTime DateUpdated { get; }
    }
}
