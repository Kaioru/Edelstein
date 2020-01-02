using System;
using System.Net;
using System.Threading.Tasks;
using DotNetty.Common.Utilities;
using Edelstein.Core.Distributed;
using Edelstein.Core.Logging;
using Edelstein.Core.Utils;
using Edelstein.Core.Utils.Packets;
using Edelstein.Entities;
using Edelstein.Entities.Characters;
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

        public long ClientKey { get; set; }
        public bool isMigrating { get; set; }

        public DateTime LastSentHeartbeatDate { get; set; }
        public DateTime LastRecvHeartbeatDate { get; set; }


        protected AbstractMigrationSocketAdapter(
            ISocket socket,
            IMigrationService service
        ) : base(socket)
        {
            _service = service;
            ClientKey = new Random().NextLong();
            LastSentHeartbeatDate = DateTime.UtcNow;
            LastRecvHeartbeatDate = DateTime.UtcNow;
        }

        public Task TryConnect()
            => _service.ProcessConnect(this);

        public Task TryDisconnect()
            => _service.ProcessDisconnect(this);

        public Task TryMigrateTo(IServerNodeState nodeState)
            => _service.ProcessMigrateTo(this, nodeState);

        public Task TryMigrateFrom(int characterID, long clientKey)
            => _service.ProcessMigrateFrom(this, characterID, clientKey);

        public Task TrySendHeartbeat()
            => _service.ProcessSendHeartbeat(this);

        public Task TryRecvHeartbeat()
            => _service.ProcessRecvHeartbeat(this);

        public virtual IPacket GetMigrationPacket(IServerNodeState to)
        {
            using var p = new Packet(SendPacketOperations.MigrateCommand);

            p.Encode<bool>(true);

            var endpoint = new IPEndPoint(IPAddress.Parse(to.Host), to.Port);
            var address = endpoint.Address.MapToIPv4().GetAddressBytes();
            var port = endpoint.Port;

            foreach (var b in address)
                p.Encode<byte>(b);
            p.Encode<short>((short) port);
            return p;
        }

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

            try
            {
                await handler.Handle(context);
                Logger.Trace($"Handled packet operation {operation}");
            }
            catch (Exception e)
            {
                await OnException(e);
            }
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