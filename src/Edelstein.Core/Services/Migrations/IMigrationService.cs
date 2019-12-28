using System.Collections.Generic;
using System.Threading.Tasks;
using Edelstein.Core.Services.Distributed;

namespace Edelstein.Core.Services.Migrations
{
    public interface IMigrationService
    {
        IDictionary<int, IMigrationSocketAdapter> Sockets { get; }

        Task ProcessMigrateTo(IMigrationSocketAdapter socketAdapter, IServerNodeState nodeState);
        Task ProcessMigrateFrom(IMigrationSocketAdapter socketAdapter, int characterID, long clientKey);
    }
}