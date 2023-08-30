using Edelstein.Protocol.Gameplay.Game.Objects;
using Edelstein.Protocol.Gameplay.Game.Objects.Mob;
using Edelstein.Protocol.Gameplay.Game.Objects.User;
using Edelstein.Protocol.Utilities.Packets;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay.Game.Handlers;

public abstract class AbstractPipedFieldMobHandler<TMessage> : AbstractPipedFieldHandler<TMessage>
{
    protected AbstractPipedFieldMobHandler(IPipeline<TMessage?> pipeline) : base(pipeline)
    {
    }

    protected override TMessage? Serialize(IFieldUser user, IPacketReader reader)
    {
        var objID = reader.ReadInt();
        var obj = user.Field?.GetPool(FieldObjectType.Mob)?.GetObject(objID);

        if (obj is not IFieldMob mob) return default;
        if (mob.Controller != user) return default;

        return Serialize(user, mob, reader);
    }

    protected abstract TMessage? Serialize(IFieldUser user, IFieldMob mob, IPacketReader reader);
}
