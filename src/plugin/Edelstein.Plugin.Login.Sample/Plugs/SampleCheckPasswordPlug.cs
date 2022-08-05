using Edelstein.Protocol.Gameplay.Stages.Login.Contracts;
using Edelstein.Protocol.Util.Pipelines;
using Microsoft.Extensions.Logging;

namespace Edelstein.Plugin.Login.Sample.Plugs;

public class SampleCheckPasswordPlug : IPipelinePlug<ICheckPassword>
{
    private readonly ILogger _logger;

    public SampleCheckPasswordPlug(ILogger logger) => _logger = logger;

    public Task Handle(IPipelineContext ctx, ICheckPassword message)
    {
        if (message.Username == "disconnect")
        {
            ctx.Cancel();
            message.User.Disconnect();
            _logger.LogInformation("Disconnected socket!");
            return Task.CompletedTask;
        }

        _logger.LogInformation("{Username} is trying to log in!", message.Username);
        return Task.CompletedTask;
    }
}
