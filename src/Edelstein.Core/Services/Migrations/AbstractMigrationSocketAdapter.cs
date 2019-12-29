using System;
using System.Threading.Tasks;
using Edelstein.Core.Distributed;
using Edelstein.Core.Logging;
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

        private readonly IMigrationService _service;

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
            _service = service;
            LastSentHeartbeatDate = DateTime.UtcNow;
            LastRecvHeartbeatDate = DateTime.UtcNow;
        }

        public Task TryMigrateTo(IServerNodeState nodeState)
            => _service.ProcessMigrateTo(this, nodeState);

        public Task TryMigrateFrom(int characterID, long clientKey)
            => _service.ProcessMigrateFrom(this, characterID, clientKey);

        public Task TrySendHeartbeat()
            => _service.ProcessSendHeartbeat(this);

        public Task TryRecvHeartbeat(bool init = false)
            => _service.ProcessRecvHeartbeat(this, init);

        public override async Task OnPacket(IPacket packet)
        {
            var operation = (RecvPacketOperations) packet.Decode<short>();

            if (!_service.Handlers.ContainsKey(operation))
            {
                Logger.Warn($"Unhandled packet operation {operation}");
                return;
            }

            var handler = _service.Handlers[operation];
            var context = new PacketHandlerContext(operation, packet, this);

            await handler.Handle(context);
            Logger.Debug($"Handled packet operation {operation}");
        }

        public override Task OnException(Exception exception)
        {
            Logger.WarnException("Socket caught exception", exception);
            return Task.CompletedTask;
        }

        public override async Task OnUpdate()
        {
            using var batch = _service.DataStore.StartBatch();

            if (Account != null) await batch.UpdateAsync(Account);
            if (AccountWorld != null) await batch.UpdateAsync(AccountWorld);
            if (Character != null) await batch.UpdateAsync(Character);

            await batch.SaveChangesAsync();
        }

        public override Task OnDisconnect()
            => _service.ProcessDisconnect(this);
    }
}