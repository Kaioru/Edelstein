using System;
using System.Threading;
using System.Threading.Tasks;
using Edelstein.Protocol.Gameplay.Login;
using Edelstein.Protocol.Services.Server;
using Edelstein.Protocol.Services.Server.Contracts;
using Edelstein.Protocol.Services.Server.Entities;
using MapsterMapper;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using R3;
using Serilog;

namespace Edelstein.Application.Server.Services;

public class SystemServerRegistryHostService<TServerInfo>(
    ILogger<SystemServerRegistryHostService<TServerInfo>> logger,
    IMapper mapper,
    IServerService service,
    TServerInfo info
) : IHostedService
    where TServerInfo : class, IServerInfo
{
    private bool IsRegistered { get; set; }
    private long Secret { get; set; }
    private IDisposable? Subscription { get; set; }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        Subscription = Observable
            .Timer(TimeSpan.Zero, TimeSpan.FromMinutes(2), cancellationToken)
            .Select(async _ =>
            {
                if (IsRegistered)
                {
                    var result = (await service.Ping(new ServerServicePingRequest
                    {
                        ID = info.ID,
                        Secret = Secret
                    })).Result;

                    if (result == ServerServiceResult.Success)
                        logger.LogSystemServerRegistryHostUpdated(info.ID);
                    else
                        logger.LogSystemServerRegistryHostUpdateFailed(info.ID, result);

                    IsRegistered = result == ServerServiceResult.Success;
                }
                else
                {
                    var response = info switch
                    {
                        ILoginStageSystemOptions login => await service.RegisterLogin(new ServerServiceRegisterRequest<ServerServiceServerInfoLogin>
                        {
                            Info = mapper.Map<ServerServiceServerInfoLogin>(login)
                        }),
                        _ => throw new ArgumentOutOfRangeException()
                    };

                    if (response.Result == ServerServiceResult.Success)
                        logger.LogSystemServerRegistryHostRegistered(info.ID);
                    else 
                        logger.LogSystemServerRegistryHostRegisterFailed(info.ID, response.Result);

                    Secret = response.Secret ?? 0;
                    IsRegistered = response.Result == ServerServiceResult.Success;
                }
            })
            .Subscribe();
        return Task.CompletedTask;
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        Subscription?.Dispose();
        if (IsRegistered)
        {
            await service.Deregister(new ServerServiceDeregisterRequest
            {
                ID = info.ID,
                Secret = Secret
            });

            logger.LogSystemServerRegistryHostDeregistered(info.ID);
        }
    }
}
