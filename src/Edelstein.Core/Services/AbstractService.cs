using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using Edelstein.Core.Services.Peers;
using Foundatio.Messaging;

namespace Edelstein.Core.Services
{
    public abstract class AbstractService<TInfo>
        where TInfo : ServiceInfo
    {
        public ICollection<ServiceInfo> Peers => _peers
            .Values
            .Where(p => p.Expiry > DateTime.Now)
            .Select(p => p.Info)
            .ToImmutableList();

        private Timer _timer;
        private readonly TInfo _info;
        private readonly IMessageBus _messageBus;
        private readonly IDictionary<string, PeerServiceInfo> _peers;

        protected AbstractService(TInfo info, IMessageBus messageBus)
        {
            _info = info;
            _messageBus = messageBus;
            _peers = new ConcurrentDictionary<string, PeerServiceInfo>();
        }

        public virtual async Task Start()
        {
            await _messageBus.SubscribeAsync<PeerServiceStatusMessage>(msg =>
            {
                if (msg.Status == PeerServiceStatus.Online)
                {
                    _peers[msg.Info.Name] = new PeerServiceInfo
                    {
                        Info = msg.Info,
                        Expiry = DateTime.Now.AddSeconds(30)
                    };
                }
                else _peers.Remove(msg.Info.Name);
            });

            var createMessage = new Func<PeerServiceStatusMessage>(() => new PeerServiceStatusMessage
            {
                Status = PeerServiceStatus.Online,
                Info = _info
            });

            _timer = new Timer
            {
                Interval = 20000,
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
                Info = _info
            });
        }
    }
}