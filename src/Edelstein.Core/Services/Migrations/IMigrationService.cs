using System.Collections.Generic;
using System.Threading.Tasks;
using Edelstein.Core.Services.Distributed;
using Edelstein.Core.Utils;
using Edelstein.Core.Utils.Packets;
using Edelstein.Database;

namespace Edelstein.Core.Services.Migrations
{
    public interface IMigrationService
    {
        IDataStore DataStore { get; }

        IDictionary<int, IMigrationSocketAdapter> Sockets { get; }
        IDictionary<RecvPacketOperations, IPacketHandler> Handlers { get; }

        Task ProcessMigrateTo(IMigrationSocketAdapter socketAdapter, IServerNodeState nodeState);
        Task ProcessMigrateFrom(IMigrationSocketAdapter socketAdapter, int characterID, long clientKey);
        Task ProcessDisconnect(IMigrationSocketAdapter socketAdapter);

        Task ProcessSendHeartbeat(IMigrationSocketAdapter socketAdapter);
        Task ProcessRecvHeartbeat(IMigrationSocketAdapter socketAdapter, bool init = false);
    }
}