using Edelstein.Protocol.Gameplay.Stages.Login.Messages;
using Edelstein.Protocol.Util.Pipelines;
using Microsoft.Extensions.Logging;

namespace Edelstein.Common.Gameplay.Stages.Login.Actions;

public class CheckPasswordAction : IPipelineAction<ICheckPassword>
{
    private readonly ILogger _logger;

    public CheckPasswordAction(ILogger<CheckPasswordAction> logger) => _logger = logger;

    public Task Handle(IPipelineContext ctx, ICheckPassword message)
    {
        _logger.LogInformation(
            "Login attempt with username: {Username}, password: {Password}",
            message.Username, message.Password
        );
        return Task.CompletedTask;
    }
}
