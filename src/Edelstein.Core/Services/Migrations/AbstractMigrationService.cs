using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Edelstein.Core.Services.Distributed;
using Edelstein.Core.Utils.Ticks;
using Edelstein.Network.Transport;
using Foundatio.Caching;
using Foundatio.Messaging;

namespace Edelstein.Core.Services.Migrations
{
    public abstract class AbstractMigrationService<TState> : NodeServerService<TState>, IMigrationService
        where TState : IServerNodeState
    {
        private readonly ITicker _ticker;

        public IDictionary<int, IMigrationSocketAdapter> Sockets { get; }

        protected AbstractMigrationService(
            TState state,
            ICacheClient cache,
            IMessageBus bus,
            Server server
        ) : base(state, cache, bus, server)
        {
            _ticker = new TimerTicker(TimeSpan.FromSeconds(1), new MigrationServiceTickBehavior(this));
            Sockets = new ConcurrentDictionary<int, IMigrationSocketAdapter>();
        }

        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            await base.StartAsync(cancellationToken);
            _ticker.Start();
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            await base.StopAsync(cancellationToken);
            _ticker.Stop();
        }

        public Task ProcessMigrateTo(IMigrationSocketAdapter socketAdapter, IServerNodeState nodeState)
        {
            // TODO
            return Task.CompletedTask;
        }

        public Task ProcessMigrateFrom(IMigrationSocketAdapter socketAdapter, int characterID, long clientKey)
        {
            // TODO
            return Task.CompletedTask;
        }
    }
}