using System;
using System.Linq;
using System.Threading.Tasks;
using Edelstein.Protocol.Services.Contracts;
using Edelstein.Protocol.Util.Ticks;
using MoreLinq;

namespace Edelstein.Common.Gameplay.Stages.Behaviors
{
    public class ServerUpdateBehavior<TStage, TUser, TConfig> : ITickerBehavior
        where TStage : AbstractServerStage<TStage, TUser, TConfig>
        where TUser : AbstractServerStageUser<TStage, TUser, TConfig>
        where TConfig : IServerStageInfo
    {
        private readonly TStage _stage;
        private bool _isInitialized = false;
        private bool _isDisconnected = false;

        public ServerUpdateBehavior(TStage stage)
        {
            _stage = stage;
        }

        public async Task OnTick(DateTime now)
        {
            var config = _stage.Info;
            var tags = config
                .GetType()
                .GetProperties()
                .ToDictionary(
                    p => p.Name.ToString(),
                    p => p.GetValue(config).ToString()
                );

            var server = new ServerContract
            {
                Id = _stage.ID,
                Host = config.Host,
                Port = config.Port
            };

            server.Metadata.Add("Type", Enum.GetName(_stage.Type));
            server.Metadata.Add(tags);

            if (_isInitialized && !_isDisconnected)
            {
                if ((await _stage.ServerRegistry
                        .Update(new UpdateServerRequest { Server = server }))
                        .Result != ServerRegistryResult.Ok)
                    _isDisconnected = true;
                else return;
            }

            var response = await _stage.ServerRegistry.Register(new RegisterServerRequest { Server = server });

            if (response.Result == ServerRegistryResult.FailedAlreadyRegistered)
            {
                await _stage.ServerRegistry.Update(new UpdateServerRequest { Server = server });
                _isInitialized = true;
                _isDisconnected = false;
            }

            if (response.Result == ServerRegistryResult.Ok)
            {
                _isInitialized = true;
                _isDisconnected = false;
            }
        }
    }
}
