using System.Collections.Generic;
using System.Threading.Tasks;
using Edelstein.Core.Distributed;
using Edelstein.Core.Utils;
using Edelstein.Core.Utils.Packets;
using Edelstein.Database;

namespace Edelstein.Core.Gameplay.Migrations
{
    public interface IMigrationService
    {
        IDataStore DataStore { get; }

        IDictionary<int, IMigrationSocketAdapter> Sockets { get; }
        IDictionary<RecvPacketOperations, IPacketHandler> Handlers { get; }

        Task ProcessConnect(IMigrationSocketAdapter adapter);
        Task ProcessDisconnect(IMigrationSocketAdapter adapter);
        
        Task ProcessMigrateTo(IMigrationSocketAdapter adapter, IServerNodeState nodeState);
        Task ProcessMigrateFrom(IMigrationSocketAdapter adapter, int characterID, long clientKey);

        Task ProcessSendHeartbeat(IMigrationSocketAdapter adapter);
        Task ProcessRecvHeartbeat(IMigrationSocketAdapter adapter);
    }
}