using System;
using System.Threading.Tasks;
using Edelstein.Core.Logging;
using Edelstein.Core.Utils.Messaging;
using Edelstein.Core.Utils.Ticks;
using Foundatio.Messaging;

namespace Edelstein.Core.Distributed
{
    public class CachedNodeLocal<TState> : AbstractCachedNode, INodeLocal<TState>
        where TState : INodeState
    {
        private static readonly ILog Logger = LogProvider.GetCurrentClassLogger();

        private readonly IMessageBusFactory _busFactory;
        private readonly IMessageBus _bus;
        private readonly ITicker _ticker;

        public TState State { get; }

        public CachedNodeLocal(TState state, IMessageBusFactory busFactory) : base(busFactory, state.Name)
        {
            _busFactory = busFactory;
            _bus = busFactory.Build(DistributedScopes.NodeSet);
            _ticker = new TimerTicker(TimeSpan.FromSeconds(10), new CachedNodeLocalTickBehavior<TState>(this, _bus));
            State = state;
        }

        protected void Start()
        {
            _bus.SubscribeAsync<CachedNodeHeartbeatMessage>((message, token) =>
            {
                if (Remotes.ContainsKey(message.State.Name))
                {
                    if (Remotes[message.State.Name].Expire < DateTime.UtcNow)
                        Logger.Debug($"Reconnected peer service, {message.State.Name} to {State.Name}");
                }
                else Logger.Debug($"Connected peer service, {message.State.Name} to {State.Name}");

                Remotes[message.State.Name] = new CachedNodeRemote(
                    message.State,
                    DateTime.UtcNow.AddMinutes(1),
                    _busFactory
                );
                return Task.CompletedTask;
            }).Wait();
            _ticker.ForceTick().Wait();
            _ticker.Start();
        }

        protected void Stop()
        {
            _ticker.Stop();
        }
    }
}