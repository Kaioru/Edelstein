using Edelstein.Common.Gameplay.Stages.Handlers;
using Edelstein.Protocol.Gameplay.Stages.Contracts;
using Edelstein.Protocol.Gameplay.Stages.Game;
using Edelstein.Protocol.Util.Pipelines;

namespace Edelstein.Common.Gameplay.Stages.Game.Handlers;

public class MigrateInHandler : AbstractMigrateInHandler<IGameStageUser>
{
    public MigrateInHandler(IPipeline<ISocketOnMigrateIn<IGameStageUser>> pipeline) : base(pipeline)
    {
    }
}
