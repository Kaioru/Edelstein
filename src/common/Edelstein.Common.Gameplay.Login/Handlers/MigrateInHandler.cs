using Edelstein.Common.Gameplay.Handlers;
using Edelstein.Protocol.Gameplay.Contracts;
using Edelstein.Protocol.Gameplay.Login;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay.Login.Handlers;

public class MigrateInHandler : AbstractMigrateInHandler<ILoginStageUser>
{
    public MigrateInHandler(IPipeline<UserOnPacketMigrateIn<ILoginStageUser>> pipeline) : base(pipeline)
    {
    }
}
