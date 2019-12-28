using System;
using System.Threading.Tasks;
using Edelstein.Core.Logging;
using Edelstein.Core.Services.Distributed;
using Edelstein.Core.Utils;
using Edelstein.Core.Utils.Packets;
using Edelstein.Entities;
using Edelstein.Network;
using Edelstein.Network.Packets;

namespace Edelstein.Core.Services.Migrations
{
    public abstract class AbstractMigrationSocketAdapter : AbstractSocketAdapter, IMigrationSocketAdapter
    {
        private static readonly ILog Logger = LogProvider.GetCurrentClassLogger();

        public IMigrationService Service { get; }

        public Account Account { get; set; }
        public AccountWorld AccountWorld { get; set; }
        public Character Character { get; set; }

        public DateTime LastSentHeartbeatDate { get; set; }
        public DateTime LastRecvHeartbeatDate { get; set; }

        protected AbstractMigrationSocketAdapter(
            ISocket socket,
            IMigrationService service
        ) : base(socket)
        {
            Service = service;
            LastSentHeartbeatDate = DateTime.UtcNow;
            LastRecvHeartbeatDate = DateTime.UtcNow;
        }

        public Task TryMigrateTo(IServerNodeState nodeState)
            => Service.ProcessMigrateTo(this, nodeState);

        public Task TryMigrateFrom(int characterID, long clientKey)
            => Service.ProcessMigrateFrom(this, characterID, clientKey);

        public Task TrySendHeartbeat()
            => Service.ProcessRecvHeartbeat(this);

        public Task TryRecvHeartbeat(bool init = false)
            => Service.ProcessRecvHeartbeat(this, init);

        public override async Task OnPacket(IPacket packet)
        {
            var operation = (RecvPacketOperations) packet.Decode<short>();

            if (!Service.Handlers.ContainsKey(operation))
            {
                Logger.Warn($"Unhandled packet operation {operation}");
                return;
            }

            var handler = Service.Handlers[operation];
            var context = new PacketHandlerContext(operation, packet, this);

            await handler.Handle(context);
        }

        public override Task OnException(Exception exception)
        {
            Logger.WarnException("Socket caught exception", exception);
            return Task.CompletedTask;
        }

        public override async Task OnUpdate()
        {
            using var batch = Service.DataStore.StartBatch();

            if (Account != null) await batch.UpdateAsync(Account);
            if (AccountWorld != null) await batch.UpdateAsync(AccountWorld);
            if (Character != null) await batch.UpdateAsync(Character);

            await batch.SaveChangesAsync();
        }

        public override Task OnDisconnect()
            => Service.ProcessDisconnect(this);
    }
}