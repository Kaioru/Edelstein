using System;
using System.Threading.Tasks;
using Edelstein.Core.Services.Distributed;
using Edelstein.Entities;
using Edelstein.Network;
using Edelstein.Network.Packets;

namespace Edelstein.Core.Services.Migrations
{
    public abstract class AbstractMigrationSocketAdapter : AbstractSocketAdapter, IMigrationSocketAdapter
    {
        public IMigrationService Service { get; }

        public Account Account { get; set; }
        public AccountWorld AccountWorld { get; set; }
        public Character Character { get; set; }

        protected AbstractMigrationSocketAdapter(
            ISocket socket,
            IMigrationService service
        ) : base(socket)
            => Service = service;

        public Task TryMigrateTo(IServerNodeState nodeState)
            => Service.ProcessMigrateTo(this, nodeState);

        public Task TryMigrateFrom(int characterID, long clientKey)
            => Service.ProcessMigrateFrom(this, characterID, clientKey);

        public override async Task OnPacket(IPacket packet)
        {
            // TODO
        }

        public override async Task OnException(Exception exception)
        {
            // TODO
        }

        public override async Task OnUpdate()
        {
            // TODO
        }

        public override async Task OnDisconnect()
        {
            // TODO
        }
    }
}