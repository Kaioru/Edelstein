using System;
using System.Threading.Tasks;
using Edelstein.Core.Distributed;
using Edelstein.Entities;
using Edelstein.Network;

namespace Edelstein.Core.Services.Migrations
{
    public interface IMigrationSocketAdapter : ISocketAdapter
    {
        Account Account { get; set; }
        AccountWorld AccountWorld { get; set; }
        Character Character { get; set; }

        DateTime LastSentHeartbeatDate { get; set; }
        DateTime LastRecvHeartbeatDate { get; set; }

        Task TryMigrateTo(IServerNodeState nodeState);
        Task TryMigrateFrom(int characterID, long clientKey);

        Task TrySendHeartbeat();
        Task TryRecvHeartbeat(bool init = false);
    }
}