using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Edelstein.Core.Logging;
using Edelstein.Core.Services.Info;
using Edelstein.Core.Services.Peers;
using Edelstein.Data.Context;
using Foundatio.Caching;
using Foundatio.Messaging;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Timer = System.Timers.Timer;

namespace Edelstein.Core.Services
{
    public abstract class AbstractHostedService<TInfo> : IHostedService
        where TInfo : ServiceInfo, new()
    {
        private static readonly ILog Logger = LogProvider.GetCurrentClassLogger();

        private Timer _timer;
        private readonly IApplicationLifetime _appLifetime;
        private readonly ICacheClient _cache;
        private readonly IMessageBus _messageBus;
        private readonly IDictionary<string, PeerServiceInfo> _peers;

        public IEnumerable<ServiceInfo> Peers => _peers
            .Values
            .Where(p => p.Expiry > DateTime.Now)
            .Select(p => p.Info)
            .ToImmutableList();

        public TInfo Info { get; }
        public IDataContextFactory DataContextFactory { get; }

        private const string MigrationCacheScope = "migration";
        private const string AccountStatusCacheScope = "accountStatus";
        public ICacheClient MigrationCache { get; }
        public ICacheClient AccountStatusCache { get; }

        public AbstractHostedService(
            IApplicationLifetime appLifetime,
            ICacheClient cache,
            IMessageBus messageBus,
            IOptions<TInfo> info,
            IDataContextFactory dataContextFactory
        )
        {
            _appLifetime = appLifetime;
            _cache = cache;
            _messageBus = messageBus;
            _peers = new ConcurrentDictionary<string, PeerServiceInfo>();

            Info = info.Value;
            DataContextFactory = dataContextFactory;

            MigrationCache = new ScopedCacheClient(_cache, MigrationCacheScope);
            AccountStatusCache = new ScopedCacheClient(_cache, AccountStatusCacheScope);
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _appLifetime.ApplicationStarted.Register(async () => await OnStarted());
            _appLifetime.ApplicationStopping.Register(async () => await OnStopping());
            _appLifetime.ApplicationStopped.Register(async () => await OnStopped());
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _appLifetime.StopApplication();
            return Task.CompletedTask;
        }

        protected virtual async Task OnStarted()
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

        protected virtual async Task OnStopping()
        {
            _timer.Stop();
            await _messageBus.PublishAsync(
                PeerServiceStatusMessage.Create(PeerServiceStatus.Offline, Info)
            );
        }

        protected virtual Task OnStopped()
            => Task.CompletedTask;
    }
}