using System;
using System.Linq;
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
        private bool _isInitialized = false;

        public ServerUpdateBehavior(TStage stage)
            => _stage = stage;

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

            if (_isInitialized)
            {
                await _stage.ServerRegistryService.UpdateServer(new UpdateServerRequest { Server = server });
                return;
            }

            await _stage.ServerRegistryService.RegisterServer(new RegisterServerRequest { Server = server });
            _isInitialized = true;
        }
    }
}
