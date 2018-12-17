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

        private const string MigrationCacheScope = "migration";
        public ICacheClient MigrationCache { get; }

        protected AbstractService(TInfo info, ICacheClient cache, IMessageBus messageBus)
        {
            Info = info;
            _cache = cache;
            _messageBus = messageBus;
            _peers = new ConcurrentDictionary<string, PeerServiceInfo>();

            MigrationCache = new ScopedCacheClient(_cache, MigrationCacheScope);
        }

        public virtual async Task Start()
        {
            await _messageBus.SubscribeAsync<PeerServiceStatusMessage>(msg =>
            {
                if (msg.Status == PeerServiceStatus.Online)
                {
                    var expiry = DateTime.Now.AddSeconds(30);

                    if (_peers.ContainsKey(msg.Info.Name))
                    {
                        var peer = _peers[msg.Info.Name];
                        
                        if (peer.Expiry < DateTime.Now)
                            Logger.Debug($"Reconnected peer service, {msg.Info.Name} to {Info.Name}");
                        _peers[msg.Info.Name].Expiry = expiry;
                    }
                    else
                    {
                        _peers[msg.Info.Name] = new PeerServiceInfo
                        {
                            Info = msg.Info,
                            Expiry = expiry
                        };
                        Logger.Debug($"Registered peer service, {msg.Info.Name} to {Info.Name}");
                    }
                }
                else
                {
                    _peers.Remove(msg.Info.Name);
                    Logger.Debug($"Removed peer service, {msg.Info.Name} from {Info.Name}");
                }
            });

            var createMessage = new Func<PeerServiceStatusMessage>(() => new PeerServiceStatusMessage
            {
                Status = PeerServiceStatus.Online,
                Info = Info
            });

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
            await _messageBus.PublishAsync(new PeerServiceStatusMessage
            {
                Status = PeerServiceStatus.Offline,
                Info = Info
            });
        }
    }
}