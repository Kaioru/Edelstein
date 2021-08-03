using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Edelstein.Protocol.Interop.Contracts;
using Edelstein.Protocol.Util.Ticks;

namespace Edelstein.Common.Gameplay.Stages.Behaviors
{
    public class ServerUpdateBehavior<TStage, TUser, TConfig> : ITickerBehavior
        where TStage : AbstractServerStage<TStage, TUser, TConfig>
        where TUser : AbstractServerStageUser<TStage, TUser, TConfig>
        where TConfig : ServerStageConfig
    {
        private readonly TStage _stage;
        private readonly CancellationTokenSource _tokenSource;
        private bool _isInitialized = false;
        private bool _isDisconnected = false;

        public ServerUpdateBehavior(TStage stage)
        {
            _stage = stage;
            _tokenSource = new CancellationTokenSource();
        }

        public async Task OnTick(DateTime now)
        {
            var config = _stage.Config;
            var tags = config
                .GetType()
                .GetProperties()
                .ToDictionary(
                    p => p.Name.ToString(),
                    p => p.GetValue(config).ToString()
                );
            var server = new ServerObject
            {
                Id = _stage.ID,
                ServerConnection = new ServerConnectionObject
                {
                    Host = config.ServerHost,
                    Port = config.ServerPort
                }
            };

            server.Tags.Add("Type", Enum.GetName(_stage.Type));
            server.Tags.Add(tags);

            if (_isInitialized && !_isDisconnected)
            {
                if ((await _stage.ServerRegistryService
                        .UpdateServer(new UpdateServerRequest { Server = server }))
                        .Result != ServiceRegistryResult.Ok)
                    _isDisconnected = true;
                else return;
            }

            var tokenSource = new CancellationTokenSource();
            var response = await _stage.ServerRegistryService.RegisterServer(new RegisterServerRequest { Server = server });

            if (response.Result == ServiceRegistryResult.Ok)
            {
                _stage.DispatchSubscriptionTask = Task.Run(async () =>
                {
                    await foreach (var dispatch in _stage.ServerRegistryService
                        .SubscribeDispatch(new DispatchSubscription { Server = _stage.ID })
                        .WithCancellation(_tokenSource.Token)
                    ) await _stage.OnNotifyDispatch(dispatch);
                }, _tokenSource.Token);
                _isInitialized = true;
                _isDisconnected = false;
            }
        }
    }
}
