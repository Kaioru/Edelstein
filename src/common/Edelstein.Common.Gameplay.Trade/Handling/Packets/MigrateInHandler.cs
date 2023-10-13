using Edelstein.Common.Gameplay.Handling.Packets;
using Edelstein.Protocol.Gameplay.Contracts;
using Edelstein.Protocol.Gameplay.Trade;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay.Trade.Handling.Packets;

public class MigrateInHandler : AbstractMigrateInHandler<ITradeStageUser>
{
    public MigrateInHandler(IPipeline<UserOnPacketMigrateIn<ITradeStageUser>> pipeline) : base(pipeline)
    {
    }
}
