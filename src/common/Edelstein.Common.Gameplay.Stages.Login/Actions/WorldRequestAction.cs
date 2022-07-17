using Edelstein.Protocol.Gameplay.Stages.Login.Messages;
using Edelstein.Protocol.Util.Pipelines;
using Microsoft.Extensions.Logging;

namespace Edelstein.Common.Gameplay.Stages.Login.Actions;

public class WorldRequestAction : IPipelineAction<IWorldRequest>
{
    private readonly ILogger _logger;

    public WorldRequestAction(ILogger<WorldRequestAction> logger) => _logger = logger;

    public Task Handle(IPipelineContext ctx, IWorldRequest message)
    {
        _logger.LogInformation("Called world request");
        return Task.CompletedTask;
    }
}
