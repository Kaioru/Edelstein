using Edelstein.Common.Gameplay.Game.Objects.Summoned;
using Edelstein.Common.Gameplay.Handling;
using Edelstein.Common.Utilities.Packets;
using Edelstein.Protocol.Gameplay.Game.Contracts;
using Edelstein.Protocol.Gameplay.Game.Objects.Summoned;
using Edelstein.Protocol.Gameplay.Game.Objects.User;
using Edelstein.Protocol.Utilities.Packets;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay.Game.Handling.Packets;

public class SummonedMoveHandler : AbstractPipedFieldSummonedHandler<FieldOnPacketSummonedMove>
{
    public SummonedMoveHandler(IPipeline<FieldOnPacketSummonedMove> pipeline) : base(pipeline)
    {
    }

    public override short Operation => (short)PacketRecvOperations.SummonedMove;

    protected override FieldOnPacketSummonedMove? Serialize(IFieldUser user, IFieldSummoned summoned, IPacketReader reader)
        => new(user, summoned, reader.Read(new FieldSummonedMovePath()));
}
