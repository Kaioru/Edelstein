using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Edelstein.Core.Distributed.Peers;
using Edelstein.Core.Logging;
using Edelstein.Core.Utils.Messaging;
using Foundatio.Caching;
using Foundatio.Messaging;
using Timer = System.Timers.Timer;

namespace Edelstein.Core.Distributed
{
    public abstract class AbstractPeerService<TInfo> : IPeerService
        where TInfo : PeerServiceInfo
    {
        private static readonly ILog Logger = LogProvider.GetCurrentClassLogger();

        public readonly TInfo Info;

        private readonly ICacheClient _cacheClient;
        private readonly IMessageBusFactory _messageBusFactory;
        public IMessageBus MessageBus { get; }

        private readonly IMessageBus _peerMessageBus;
        private readonly IDictionary<string, PeerServiceInfoEntry> _peers;

        public IEnumerable<PeerServiceInfo> Peers => _peers
            .Values
            .Where(p => p.Expiry > DateTime.Now)
            .Select(p => p.Info)
            .ToImmutableList();

        private Timer _peerTimer;
        private Timer _tickTimer;

        public AbstractPeerService(TInfo info, ICacheClient cacheClient, IMessageBusFactory messageBusFactory)
        {
            Info = info;
            _cacheClient = cacheClient;
            _messageBusFactory = messageBusFactory;

            MessageBus = messageBusFactory.Build($"{Scopes.PeerMessaging}:{info.Name}");

            _peerMessageBus = messageBusFactory.Build(Scopes.PeerDiscovery);
            _peers = new ConcurrentDictionary<string, PeerServiceInfoEntry>();
        }

        public Task StartAsync(CancellationToken cancellationToken) => OnStart();
        public Task StopAsync(CancellationToken cancellationToken) => OnStop();

        public virtual async Task OnStart()
        {
            await _peerMessageBus.SubscribeAsync<PeerServiceStatusMessage>(msg =>
            {
                switch (msg.Status)
                {
                    case PeerServiceStatus.Online:
                        var expiry = DateTime.Now.AddSeconds(30);

                        if (_peers.ContainsKey(msg.Info.Name))
                        {
                            var peer = _peers[msg.Info.Name];

                            if (peer.Expiry < DateTime.Now)
                                Logger.Debug($"Reconnected peer service, {msg.Info.Name} to {Info.Name}");
                            _peers[msg.Info.Name].Expiry = expiry;
                            break;
                        }

                        _peers[msg.Info.Name] = new PeerServiceInfoEntry
                        {
                            Info = msg.Info,
                            Expiry = expiry
                        };
                        Logger.Debug($"Connected peer service, {msg.Info.Name} to {Info.Name}");
                        break;
                    case PeerServiceStatus.Offline:
                    default:
                        _peers.Remove(msg.Info.Name);
                        Logger.Debug($"Removed peer service, {msg.Info.Name} from {Info.Name}");
                        break;
                }
            });

            _peerTimer = new Timer
            {
                Interval = 15000,
                AutoReset = true
            };
            _peerTimer.Elapsed += async (sender, args) => await _peerMessageBus.PublishAsync(
                new PeerServiceStatusMessage
                {
                    Info = Info,
                    Status = PeerServiceStatus.Online
                }
            );
            _peerTimer.Start();

            await _peerMessageBus.PublishAsync(new PeerServiceStatusMessage
            {
                Info = Info,
                Status = PeerServiceStatus.Online
            });

            _tickTimer = new Timer
            {
                Interval = 1000,
                AutoReset = true
            };
            _tickTimer.Elapsed += async (sender, args) => await OnTick(DateTime.Now);
            _tickTimer.Start();
        }

        public virtual async Task OnStop()
        {
            _peerTimer.Stop();
            await _peerMessageBus.PublishAsync(new PeerServiceStatusMessage
            {
                Info = Info,
                Status = PeerServiceStatus.Offline
            });
        }

        public abstract Task OnTick(DateTime now);

        public Task BroadcastMessage<T>(T message) where T : class
            => Task.WhenAll(Peers.Select(p => SendMessage(p, message)));

        public Task SendMessage<T>(PeerServiceInfo info, T message) where T : class
            => _messageBusFactory
                .Build($"{Scopes.PeerMessaging}:{info.Name}")
                .PublishAsync<T>(message);
    }
}