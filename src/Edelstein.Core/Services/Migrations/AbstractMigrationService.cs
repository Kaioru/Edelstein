using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Edelstein.Core.Distributed;
using Edelstein.Core.Utils;
using Edelstein.Core.Utils.Messaging;
using Edelstein.Core.Utils.Packets;
using Edelstein.Core.Utils.Ticks;
using Edelstein.Database;
using Edelstein.Network.Packets;
using Foundatio.Caching;

namespace Edelstein.Core.Services.Migrations
{
    public abstract class AbstractMigrationService<TState> : AbstractNodeServerService<TState>, IMigrationService
        where TState : IServerNodeState
    {
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
            _ticker = new TimerTicker(TimeSpan.FromSeconds(15), new MigrationServiceTickBehavior(this));

            DataStore = dataStore;

            AccountStateCache = new ScopedCacheClient(cache, Scopes.StateAccount);
            CharacterStateCache = new ScopedCacheClient(cache, Scopes.StateCharacter);
            MigrationCache = new ScopedCacheClient(cache, Scopes.NodeMigration);
            SocketCountCache = new ScopedCacheClient(cache, Scopes.NodeSocketCount);

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

        public async Task ProcessMigrateTo(IMigrationSocketAdapter socketAdapter, IServerNodeState nodeState)
        {
            if (socketAdapter.Account == null || socketAdapter.Character == null)
                throw new Exception("account or character is null");
            if (await MigrationCache.ExistsAsync(socketAdapter.Character.ID.ToString()))
                throw new Exception("Already migrating");

            Sockets.Remove(socketAdapter.Account.ID);

            await socketAdapter.OnUpdate();
            await AccountStateCache.SetAsync(
                socketAdapter.Account.ID.ToString(),
                MigrationState.Migrating,
                TimeSpan.FromSeconds(15)
            );
            await MigrationCache.SetAsync(
                socketAdapter.Character.ID.ToString(),
                new MigrationEntry
                {
                    Account = socketAdapter.Account,
                    AccountWorld = socketAdapter.AccountWorld,
                    Character = socketAdapter.Character,
                    ClientKey = socketAdapter.Socket.ClientKey,
                    From = State,
                    To = nodeState
                },
                TimeSpan.FromSeconds(15)
            );
            await socketAdapter.SendPacket(socketAdapter.GetMigrationPacket(nodeState));
            await SocketCountCache.DecrementAsync(State.Name);
        }

        public async Task ProcessMigrateFrom(IMigrationSocketAdapter socketAdapter, int characterID, long clientKey)
        {
            if (!await MigrationCache.ExistsAsync(characterID.ToString()))
                throw new Exception("Not migrating");

            var entry = (await MigrationCache.GetAsync<MigrationEntry>(characterID.ToString())).Value;

            if (State.Name != entry.To.Name)
                throw new Exception("Migration target is not the current service");
            if (clientKey != entry.ClientKey)
                throw new Exception("Migration client key is invalid");

            socketAdapter.Account = entry.Account;
            socketAdapter.AccountWorld = entry.AccountWorld;
            socketAdapter.Character = entry.Character;
            socketAdapter.Socket.ClientKey = entry.ClientKey;

            await MigrationCache.RemoveAsync(characterID.ToString());
            await socketAdapter.TryRecvHeartbeat();
            await SocketCountCache.IncrementAsync(State.Name);

            Sockets.Add(socketAdapter.Account.ID, socketAdapter);
        }

        public async Task ProcessDisconnect(IMigrationSocketAdapter socketAdapter)
        {
            if (socketAdapter.Account == null) return;
            var state = (await AccountStateCache.GetAsync<MigrationState>(socketAdapter.Account.ID.ToString())).Value;
            if (state != MigrationState.Migrating)
            {
                Sockets.Remove(socketAdapter.Account.ID);
                await socketAdapter.OnUpdate();
                if (socketAdapter.Account != null)
                    await AccountStateCache.RemoveAsync(socketAdapter.Account.ID.ToString());
                if (socketAdapter.Character != null)
                    await CharacterStateCache.RemoveAsync(socketAdapter.Character.ID.ToString());
            }
        }

        public async Task ProcessSendHeartbeat(IMigrationSocketAdapter socketAdapter)
        {
            if ((DateTime.UtcNow - socketAdapter.LastRecvHeartbeatDate).TotalMinutes >= 1)
            {
                await socketAdapter.Socket.Close();
                return;
            }

            if ((DateTime.UtcNow - socketAdapter.LastSentHeartbeatDate).TotalSeconds >= 30)
            {
                using var p = new Packet(SendPacketOperations.AliveReq);
                socketAdapter.LastSentHeartbeatDate = DateTime.UtcNow;
                await socketAdapter.Socket.SendPacket(p);
            }
        }

        public async Task ProcessRecvHeartbeat(IMigrationSocketAdapter socketAdapter, bool init = false)
        {
            socketAdapter.LastRecvHeartbeatDate = DateTime.UtcNow;

            if (socketAdapter.Account == null) return;
            if (!await AccountStateCache.ExistsAsync(socketAdapter.Account.ID.ToString()) && !init)
            {
                await socketAdapter.Socket.Close();
                return;
            }

            if (init)
                Sockets.Add(socketAdapter.Account.ID, socketAdapter);

            await AccountStateCache.SetAsync(
                socketAdapter.Account.ID.ToString(),
                MigrationState.LoggedIn,
                TimeSpan.FromMinutes(1)
            );

            if (socketAdapter.Character == null) return;

            await CharacterStateCache.SetAsync(
                socketAdapter.Character.ID.ToString(),
                State,
                TimeSpan.FromMinutes(1)
            );
        }
    }
}