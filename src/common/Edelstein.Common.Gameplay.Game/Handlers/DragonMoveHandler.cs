using Edelstein.Common.Gameplay.Game.Objects.Dragon;
using Edelstein.Common.Gameplay.Packets;
using Edelstein.Common.Utilities.Packets;
using Edelstein.Protocol.Gameplay.Game.Contracts;
using Edelstein.Protocol.Gameplay.Game.Objects.Dragon;
using Edelstein.Protocol.Gameplay.Game.Objects.User;
using Edelstein.Protocol.Utilities.Packets;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay.Game.Handlers;

public class DragonMoveHandler : AbstractPipedFieldDragonHandler<FieldOnPacketDragonMove>
{
    public DragonMoveHandler(IPipeline<FieldOnPacketDragonMove> pipeline) : base(pipeline)
    {
    }

    public override short Operation => (short)PacketRecvOperations.DragonMove;

    protected override FieldOnPacketDragonMove? Serialize(IFieldUser user, IFieldDragon dragon, IPacketReader reader)
        => new(user, dragon, reader.Read(new FieldDragonMovePath()));
}
