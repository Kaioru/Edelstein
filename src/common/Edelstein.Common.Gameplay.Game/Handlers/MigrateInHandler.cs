using Edelstein.Common.Gameplay.Handlers;
using Edelstein.Protocol.Gameplay.Contracts;
using Edelstein.Protocol.Gameplay.Game;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay.Game.Handlers;

public class MigrateInHandler : AbstractMigrateInHandler<IGameStageUser>
{
    public MigrateInHandler(IPipeline<UserOnPacketMigrateIn<IGameStageUser>> pipeline) : base(pipeline)
    {
    }
}
