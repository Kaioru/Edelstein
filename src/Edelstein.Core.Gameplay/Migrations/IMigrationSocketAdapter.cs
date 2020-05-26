using System;
using System.Threading.Tasks;
using Edelstein.Core.Distributed;
using Edelstein.Core.Entities;
using Edelstein.Core.Entities.Characters;
using Edelstein.Core.Network;
using Edelstein.Core.Network.Packets;

namespace Edelstein.Core.Gameplay.Migrations
{
    public interface IMigrationSocketAdapter : ISocketAdapter
    {
        Account Account { get; set; }
        AccountWorld AccountWorld { get; set; }
        Character Character { get; set; }

        long ClientKey { get; set; }
        bool isMigrating { get; set; }

        string LastConnectedService { get; set; }

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