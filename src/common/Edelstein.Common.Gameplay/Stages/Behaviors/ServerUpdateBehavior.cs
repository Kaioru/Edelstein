using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Edelstein.Protocol.Services.Contracts;
using Edelstein.Protocol.Util.Ticks;
using Microsoft.Extensions.Logging;
using MoreLinq;

namespace Edelstein.Common.Gameplay.Stages.Behaviors
{
    public class ServerUpdateBehavior<TStage, TUser, TConfig> : ITickerBehavior
        where TStage : AbstractServerStage<TStage, TUser, TConfig>
        where TUser : AbstractServerStageUser<TStage, TUser, TConfig>
        where TConfig : ServerStageInfo
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
            var server = new ServerContract
            {
                Id = _stage.ID,
                Host = _stage.Info.Host,
                Port = _stage.Info.Port
            };
            var metadata = new Dictionary<string, string>();

            _stage.Info.AddMetadata(metadata);

            server.Metadata.Add("Type", Enum.GetName(_stage.Type));
            server.Metadata.Add(metadata);

            if (_isInitialized && !_isDisconnected)
            {
                if ((await _stage.ServerRegistry
                        .Update(new UpdateServerRequest { Server = server }))
                        .Result != ServerRegistryResult.Ok)
                    _isDisconnected = true;
                else return;
            }

            var response = await _stage.ServerRegistry.Register(new RegisterServerRequest { Server = server });

            if (response.Result == ServerRegistryResult.Ok)
            {
                _isInitialized = true;
                _isDisconnected = false;

                _stage.Logger.LogInformation($"Successfully registered server {_stage.ID} to the server registry");
            }
            else
                _stage.Logger.LogInformation($"Failed to register server {_stage.ID} to the server registry due to {response.Result}");
        }
    }
}
