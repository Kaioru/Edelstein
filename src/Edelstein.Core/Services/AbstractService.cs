using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using Edelstein.Core.Logging;
using Edelstein.Core.Services.Info;
using Edelstein.Core.Services.Peers;
using Edelstein.Data.Context;
using Foundatio.Caching;
using Foundatio.Messaging;
using Serilog;

namespace Edelstein.Core.Services
{
    public abstract class AbstractService<TInfo> : IService
        where TInfo : ServiceInfo
    {
        private static readonly ILog Logger = LogProvider.GetCurrentClassLogger();

        public TInfo Info { get; }

        public ICollection<ServiceInfo> Peers => _peers
            .Values
            .Where(p => p.Expiry > DateTime.Now)
            .Select(p => p.Info)
            .ToImmutableList();

        private Timer _timer;
        private readonly ICacheClient _cache;
        private readonly IMessageBus _messageBus;
        private readonly IDictionary<string, PeerServiceInfo> _peers;

        public IDataContextFactory DataContextFactory { get; }

        private const string MigrationCacheScope = "migration";
        private const string AccountStatusCacheScope = "accountStatus";
        public ICacheClient MigrationCache { get; }
        public ICacheClient AccountStatusCache { get; }

        protected AbstractService(
            TInfo info,
            ICacheClient cache,
            IMessageBus messageBus,
            IDataContextFactory dataContextFactory
        )
        {
            Info = info;
            _cache = cache;
            _messageBus = messageBus;
            _peers = new ConcurrentDictionary<string, PeerServiceInfo>();

            DataContextFactory = dataContextFactory;

            MigrationCache = new ScopedCacheClient(_cache, MigrationCacheScope);
            AccountStatusCache = new ScopedCacheClient(_cache, AccountStatusCacheScope);
        }

        public virtual async Task Start()
        {
            await _messageBus.SubscribeAsync<PeerServiceStatusMessage>(msg =>
            {
                var info = msg.GetInfo();

                if (msg.Status == PeerServiceStatus.Online)
                {
                    var expiry = DateTime.Now.AddSeconds(30);

                    if (_peers.ContainsKey(info.Name))
                    {
                        var peer = _peers[info.Name];

                        if (peer.Expiry < DateTime.Now)
                            Logger.Debug($"Reconnected peer service, {info.Name} to {Info.Name}");
                        _peers[info.Name].Expiry = expiry;
                    }
                    else
                    {
                        _peers[info.Name] = new PeerServiceInfo
                        {
                            Info = info,
                            Expiry = expiry
                        };
                        Logger.Debug($"Registered peer service, {info.Name} to {Info.Name}");
                    }
                }
                else
                {
                    _peers.Remove(info.Name);
                    Logger.Debug($"Removed peer service, {info.Name} from {Info.Name}");
                }
            });

            var createMessage =
                new Func<PeerServiceStatusMessage>(() =>
                    PeerServiceStatusMessage.Create(PeerServiceStatus.Online, Info)
                );

            _timer = new Timer
            {
                Interval = 15000,
                AutoReset = true
            };
            _timer.Elapsed += async (sender, args) => await _messageBus.PublishAsync(createMessage());
            _timer.Start();

            await _messageBus.PublishAsync(createMessage());
        }

        public virtual async Task Stop()
        {
            _timer.Stop();
            await _messageBus.PublishAsync(
                PeerServiceStatusMessage.Create(PeerServiceStatus.Offline, Info)
            );
        }
    }
}