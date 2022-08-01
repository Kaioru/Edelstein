using Edelstein.Common.Services.Server.Contracts;
using Edelstein.Common.Services.Server.Types;
using Edelstein.Daemon.Server.Configs;
using Edelstein.Protocol.Gameplay.Stages.Game.Contexts;
using Edelstein.Protocol.Gameplay.Stages.Login.Contexts;
using Edelstein.Protocol.Services.Server;
using Edelstein.Protocol.Services.Server.Contracts;
using Edelstein.Protocol.Services.Server.Types;
using Edelstein.Protocol.Util.Tickers;
using Microsoft.Extensions.Logging;

namespace Edelstein.Daemon.Server.Bootstraps;

public class ServerUpdateBootstrap<TConfig> : IBootstrap, ITickable where TConfig : AbstractProgramConfigStage
{
    private readonly TConfig _config;
    private readonly ILogger _logger;
    private readonly IServerService _service;
    private readonly ITickerManager _ticker;

    public ServerUpdateBootstrap(
        ILogger<ServerUpdateBootstrap<TConfig>> logger,
        TConfig config,
        IServerService service,
        ITickerManager ticker)
    {
        _logger = logger;
        _config = config;
        _service = service;
        _ticker = ticker;
    }

    private ITickerManagerContext? Context { get; set; }
    private bool IsRegistered { get; set; }

    public async Task Start()
    {
        Context = _ticker.Schedule(this, TimeSpan.FromMinutes(2));
        await Context.OnTick(DateTime.UtcNow);
    }

    public async Task Stop()
    {
        Context?.Cancel();
        await _service.Deregister(new ServerDeregisterRequest(_config.ID));
        _logger.LogInformation(
            "Deregistered stage {ID} to server registry",
            _config.ID
        );
    }

    public async Task OnTick(DateTime now)
    {
        var response = IsRegistered
            ? await _service.Ping(new ServerPingRequest(_config.ID))
            : _config switch
            {
                ILoginContextOptions login => await _service.RegisterLogin(new ServerRegisterRequest<IServerLogin>(
                    new ServerLogin
                    {
                        ID = _config.ID,
                        Host = _config.Host,
                        Port = _config.Port
                    })),
                IGameContextOptions game => await _service.RegisterGame(new ServerRegisterRequest<IServerGame>(
                    new ServerGame
                    {
                        ID = _config.ID,
                        Host = _config.Host,
                        Port = _config.Port,
                        WorldID = game.WorldID,
                        ChannelID = game.ChannelID,
                        IsAdultChannel = game.IsAdultChannel
                    })),
                _ => throw new ArgumentOutOfRangeException()
            };

        if (response.Result == ServerResult.Success)
        {
            if (!IsRegistered)
            {
                IsRegistered = true;
                _logger.LogInformation(
                    "Registered stage {ID} to server registry, updating at {Next}",
                    _config.ID, Context?.TickNext
                );
            }
            else
            {
                _logger.LogDebug(
                    "Updated stage {ID} in server registry, updating at {Next}",
                    _config.ID, Context?.TickNext
                );
            }
        }
        else
        {
            if (IsRegistered)
            {
                _logger.LogWarning(
                    "Failed to updated stage {ID} in server registry due to {Reason} retrying at {Next}",
                    _config.ID, response.Result, Context?.TickNext
                );
                return;
            }

            _logger.LogWarning(
                "Failed to register stage {ID} to server registry due to {Reason} retrying at {Next}",
                _config.ID, response.Result, Context?.TickNext
            );
        }
    }
}
