using System;
using System.Threading.Tasks;
using Edelstein.Core.Distributed;
using Edelstein.Entities;
using Edelstein.Entities.Characters;
using Edelstein.Network;
using Edelstein.Network.Packets;

namespace Edelstein.Core.Services.Migrations
{
    public interface IMigrationSocketAdapter : ISocketAdapter
    {
        Account Account { get; set; }
        AccountWorld AccountWorld { get; set; }
        Character Character { get; set; }
        
        bool isMigrating { get; set; }

        DateTime LastSentHeartbeatDate { get; set; }
        DateTime LastRecvHeartbeatDate { get; set; }

        Task TryConnect();
        Task TryDisconnect();
        
        Task TryMigrateTo(IServerNodeState nodeState);
        Task TryMigrateFrom(int characterID, long clientKey);

        Task TrySendHeartbeat();
        Task TryRecvHeartbeat();

        IPacket GetMigrationPacket(IServerNodeState to);
    }
}