using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Edelstein.Core.Distributed;
using Edelstein.Core.Gameplay.Logging;
using Edelstein.Core.Services;
using Edelstein.Core.Utils;
using Edelstein.Core.Utils.Messaging;
using Edelstein.Core.Utils.Packets;
using Edelstein.Core.Utils.Ticks;
using Edelstein.Database;
using Edelstein.Network.Packets;
using Foundatio.Caching;

namespace Edelstein.Core.Gameplay.Migrations
{
    public abstract class AbstractMigrationService<TState> : AbstractNodeServerService<TState>, IMigrationService
        where TState : IServerNodeState
    {
        private static readonly ILog Logger = LogProvider.GetCurrentClassLogger();

        private readonly ITicker _ticker;

        public IDataStore DataStore { get; }
        public ICacheClient AccountStateCache { get; }
        public ICacheClient CharacterStateCache { get; }
        public ICacheClient MigrationCache { get; }
        public ICacheClient SocketCountCache { get; }

        public IDictionary<int, IMigrationSocketAdapter> Sockets { get; }
        public IDictionary<RecvPacketOperations, IPacketHandler> Handlers { get; }

        protected AbstractMigrationService(
            TState state,
            IDataStore dataStore,
            ICacheClient cache,
            IMessageBusFactory busFactory
        ) : base(state, busFactory)
        {
            _ticker = new TimerTicker(TimeSpan.FromSeconds(10), new MigrationServiceTickBehavior(this));

            DataStore = dataStore;

            AccountStateCache = new ScopedCacheClient(cache, MigrationScopes.StateAccount);
            CharacterStateCache = new ScopedCacheClient(cache, MigrationScopes.StateCharacter);
            MigrationCache = new ScopedCacheClient(cache, MigrationScopes.NodeMigration);
            SocketCountCache = new ScopedCacheClient(cache, MigrationScopes.NodeSocketCount);

            Sockets = new ConcurrentDictionary<int, IMigrationSocketAdapter>();
            Handlers = new ConcurrentDictionary<RecvPacketOperations, IPacketHandler>
            {
                [RecvPacketOperations.AliveAck] = new ActionPacketHandler(ctx =>
                {
                    if (ctx.Adapter is AbstractMigrationSocketAdapter adapter)
                        adapter.TryRecvHeartbeat();
                }),
            };
        }

        public async Task ProcessConnect(IMigrationSocketAdapter adapter)
        {
            if (adapter.Account == null) return;

            var expire = DateTime.UtcNow.AddMinutes(1);

            await AccountStateCache.SetAsync(
                adapter.Account.ID.ToString(),
                MigrationState.LoggedIn,
                expire
            );

            if (adapter.Character != null)
                await CharacterStateCache.SetAsync(
                    adapter.Character.ID.ToString(),
                    State,
                    expire
                );

            Sockets.Add(adapter.Account.ID, adapter);
            await SocketCountCache.IncrementAsync(State.Name);
        }


        public async Task ProcessDisconnect(IMigrationSocketAdapter adapter)
        {
            if (adapter.Account == null) return;
            if (!adapter.isMigrating)
            {
                await adapter.OnUpdate();

                if (adapter.Account != null)
                    await AccountStateCache.RemoveAsync(adapter.Account.ID.ToString());
                if (adapter.Character != null)
                    await CharacterStateCache.RemoveAsync(adapter.Character.ID.ToString());
            }

            Sockets.Remove(adapter.Account.ID);
            await SocketCountCache.DecrementAsync(State.Name);
        }

        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            await base.StartAsync(cancellationToken);
            await SocketCountCache.RemoveAsync(State.Name);
            _ticker.Start();
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            await base.StopAsync(cancellationToken);
            _ticker.Stop();
        }

        public async Task ProcessMigrateTo(IMigrationSocketAdapter adapter, IServerNodeState nodeState)
        {
            if (adapter.Account == null || adapter.Character == null)
                throw new Exception("account or character is null");
            if (await MigrationCache.ExistsAsync(adapter.Character.ID.ToString()))
                throw new Exception("Already migrating");

            var expiry = DateTime.UtcNow.AddSeconds(15);

            adapter.isMigrating = true;
            await AccountStateCache.SetAsync(
                adapter.Account.ID.ToString(),
                MigrationState.Migrating,
                expiry
            );
            await MigrationCache.SetAsync(
                adapter.Character.ID.ToString(),
                new MigrationEntry
                {
                    Account = adapter.Account,
                    AccountWorld = adapter.AccountWorld,
                    Character = adapter.Character,
                    ClientKey = adapter.ClientKey,
                    From = State,
                    To = nodeState
                },
                expiry
            );
            await adapter.SendPacket(adapter.GetMigrationPacket(nodeState));

            Logger.Debug($"Migrating {adapter.Character.Name} from {State.Name} to {nodeState.Name}");
        }

        public async Task ProcessMigrateFrom(IMigrationSocketAdapter adapter, int characterID, long clientKey)
        {
            if (!await MigrationCache.ExistsAsync(characterID.ToString()))
                throw new Exception("Not migrating");

            var entry = (await MigrationCache.GetAsync<MigrationEntry>(characterID.ToString())).Value;

            if (State.Name != entry.To.Name)
                throw new Exception("Migration target is not the current service");
            if (clientKey != entry.ClientKey)
                throw new Exception("Migration client key is invalid");

            adapter.Account = entry.Account;
            adapter.AccountWorld = entry.AccountWorld;
            adapter.Character = entry.Character;
            adapter.ClientKey = entry.ClientKey;
            adapter.LastConnectedService = entry.From.Name;

            await adapter.TryConnect();
            await MigrationCache.RemoveAsync(characterID.ToString());

            Logger.Debug($"Migrated {adapter.Character.Name} {entry.From.Name} to {entry.To.Name}");
        }

        public async Task ProcessSendHeartbeat(IMigrationSocketAdapter adapter)
        {
            var now = DateTime.UtcNow;

            if ((now - adapter.LastRecvHeartbeatDate).TotalMinutes >= 1)
            {
                await adapter.Close();
                Logger.Debug($"Closed connection from {adapter.Account.Username} due to heartbeat timeout");
                return;
            }

            if ((now - adapter.LastSentHeartbeatDate).TotalSeconds >= 30)
            {
                adapter.LastSentHeartbeatDate = now;

                using var p = new Packet(SendPacketOperations.AliveReq);
                await adapter.SendPacket(p);
            }
        }

        public async Task ProcessRecvHeartbeat(IMigrationSocketAdapter adapter)
        {
            adapter.LastRecvHeartbeatDate = DateTime.UtcNow;

            if (adapter.Account == null) return;
            if (!await AccountStateCache.ExistsAsync(adapter.Account.ID.ToString()))
            {
                await adapter.Close();
                Logger.Debug($"Closed connection from {adapter.Account.Username} due to cache expiry");
                return;
            }

            await AccountStateCache.SetExpirationAsync(
                adapter.Account.ID.ToString(),
                adapter.LastSentHeartbeatDate.AddMinutes(1)
            );

            if (adapter.Character == null) return;

            await CharacterStateCache.SetExpirationAsync(
                adapter.Character.ID.ToString(),
                adapter.LastSentHeartbeatDate.AddMinutes(1)
            );
        }
    }
}