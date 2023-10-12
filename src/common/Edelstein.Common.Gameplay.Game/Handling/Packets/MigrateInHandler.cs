using Edelstein.Common.Gameplay.Handling.Packets;
using Edelstein.Protocol.Gameplay.Contracts;
using Edelstein.Protocol.Gameplay.Game;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay.Game.Handling.Packets;

public class MigrateInHandler : AbstractMigrateInHandler<IGameStageUser>
{
    public MigrateInHandler(IPipeline<UserOnPacketMigrateIn<IGameStageUser>> pipeline) : base(pipeline)
    {
    }
}
