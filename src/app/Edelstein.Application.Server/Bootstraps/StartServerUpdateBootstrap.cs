using AutoMapper;
using Edelstein.Application.Server.Configs;
using Edelstein.Protocol.Gameplay.Game;
using Edelstein.Protocol.Gameplay.Login;
using Edelstein.Protocol.Gameplay.Shop;
using Edelstein.Protocol.Gameplay.Trade;
using Edelstein.Protocol.Services.Server;
using Edelstein.Protocol.Services.Server.Contracts;
using Edelstein.Protocol.Utilities.Tickers;
using Microsoft.Extensions.Logging;

namespace Edelstein.Application.Server.Bootstraps;

public class StartServerUpdateBootstrap<TConfig> : IBootstrap, ITickable
    where TConfig : ProgramConfigStage
{
    private readonly TConfig _config;
    private readonly ILogger _logger;
    private readonly IServerService _service;
    private readonly ITickerManager _ticker;

    public StartServerUpdateBootstrap(
        ILogger<StartServerUpdateBootstrap<TConfig>> logger,
        TConfig config,
        IServerService service,
        ITickerManager ticker
    )
    {
        _logger = logger;
        _config = config;
        _service = service;
        _ticker = ticker;
    }

    private ITickerManagerContext? Context { get; set; }
    private bool IsRegistered { get; set; }

    public int Priority => BootstrapPriority.Start;


    public Task Start() => Task.FromResult(Context = _ticker.Schedule(this, TimeSpan.FromMinutes(2), TimeSpan.Zero));

    public async Task Stop()
    {
        Context?.Cancel();

        if (IsRegistered)
        {
            await _service.Deregister(new ServerDeregisterRequest(_config.ID));
            _logger.LogInformation(
                "Deregistered stage {ID} from server registry",
                _config.ID
            );
        }
    }

    public Task OnTick(DateTime now)
    {
        _ = Task.Run(async () =>
        {
            var response = IsRegistered
                ? await _service.Ping(new ServerPingRequest(_config.ID))
                : _config switch
                {
                    ILoginStageOptions login => await _service.RegisterLogin(new ServerRegisterRequest<IServerLogin>(
                        new ServerLogin(_config.ID, _config.Host, _config.Port))),
                    IGameStageOptions game => await _service.RegisterGame(new ServerRegisterRequest<IServerGame>(
                        new ServerGame(_config.ID, _config.Host, _config.Port, game.WorldID, game.ChannelID, game.IsAdultChannel))),
                    IShopStageOptions shop => await _service.RegisterShop(new ServerRegisterRequest<IServerShop>(
                        new ServerShop(_config.ID, _config.Host, _config.Port, shop.WorldID))),
                    ITradeStageOptions trade => await _service.RegisterTrade(new ServerRegisterRequest<IServerTrade>(
                        new ServerTrade(_config.ID, _config.Host, _config.Port, trade.WorldID))),
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
                        "Failed to update stage {ID} in server registry due to {Reason} retrying at {Next}",
                        _config.ID, response.Result, Context?.TickNext
                    );

                    if (response.Result == ServerResult.FailedNotRegistered)
                        IsRegistered = false;
                    return;
                }

                _logger.LogWarning(
                    "Failed to register stage {ID} to server registry due to {Reason} retrying at {Next}",
                    _config.ID, response.Result, Context?.TickNext
                );
            }
        });

        return Task.CompletedTask;
    }
}
