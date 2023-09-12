using Edelstein.Common.Gameplay.Game.Objects.Mob;
using Edelstein.Common.Gameplay.Packets;
using Edelstein.Common.Utilities.Packets;
using Edelstein.Protocol.Gameplay.Game.Contracts;
using Edelstein.Protocol.Gameplay.Game.Objects.Mob;
using Edelstein.Protocol.Gameplay.Game.Objects.User;
using Edelstein.Protocol.Utilities.Packets;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay.Game.Handlers;

public class MobMoveHandler : AbstractPipedFieldMobHandler<FieldOnPacketMobMove>
{
    public MobMoveHandler(IPipeline<FieldOnPacketMobMove?> pipeline) : base(pipeline)
    {
    }

    public override short Operation => (short)PacketRecvOperations.MobMove;

    protected override FieldOnPacketMobMove? Serialize(IFieldUser user, IFieldMob mob, IPacketReader reader)
        => new(user, mob, reader.Read(new FieldMobMovePath()));
}
