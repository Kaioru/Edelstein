using Edelstein.Protocol.Gameplay.Game.Objects;
using Edelstein.Protocol.Gameplay.Game.Objects.Summoned;
using Edelstein.Protocol.Gameplay.Game.Objects.User;
using Edelstein.Protocol.Utilities.Packets;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay.Game.Handlers;

public abstract class AbstractPipedFieldSummonedHandler<TMessage> : AbstractPipedFieldHandler<TMessage>
{
    protected AbstractPipedFieldSummonedHandler(IPipeline<TMessage> pipeline) : base(pipeline)
    {
    }

    protected override TMessage? Serialize(IFieldUser user, IPacketReader reader)
    {
        var objID = reader.ReadInt();
        var obj = user.Field?.GetPool(FieldObjectType.Summoned)?.GetObject(objID);

        if (obj is not IFieldSummoned summoned) return default;
        if (summoned.Owner != user) return default;

        return Serialize(user, summoned, reader);
    }

    protected abstract TMessage? Serialize(IFieldUser user, IFieldSummoned summoned, IPacketReader reader);
}
