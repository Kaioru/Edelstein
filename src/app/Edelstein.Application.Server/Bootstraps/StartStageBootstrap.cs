using Edelstein.Protocol.Gameplay.Contracts;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Application.Server.Bootstraps;

public class StartStageBootstrap : IBootstrap
{
    private readonly IPipeline<StageStart> _stageStart;
    private readonly IPipeline<StageStop> _stageStop;

    public int Priority => BootstrapPriority.Start;

    public StartStageBootstrap(IPipeline<StageStart> stageStart, IPipeline<StageStop> stageStop)
    {
        _stageStart = stageStart;
        _stageStop = stageStop;
    }

    public Task Start() => _stageStart.Process(new StageStart());
    public Task Stop() => _stageStop.Process(new StageStop());
}
