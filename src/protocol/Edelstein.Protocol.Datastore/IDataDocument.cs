using System;
using Edelstein.Protocol.Util.Repositories;

namespace Edelstein.Protocol.Datastore
{
    public interface IDataDocument : IRepositoryEntry<int>
    {
        DateTime DateDocumentCreated { get; set; }
        DateTime DateDocumentUpdated { get; set; }
    }
}
