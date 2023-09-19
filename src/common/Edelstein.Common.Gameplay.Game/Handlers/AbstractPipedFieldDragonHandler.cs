using Edelstein.Protocol.Gameplay.Game.Objects.Dragon;
using Edelstein.Protocol.Gameplay.Game.Objects.User;
using Edelstein.Protocol.Utilities.Packets;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay.Game.Handlers;

public abstract class AbstractPipedFieldDragonHandler<TMessage> : AbstractPipedFieldHandler<TMessage>
{
    protected AbstractPipedFieldDragonHandler(IPipeline<TMessage> pipeline) : base(pipeline)
    {
    }

    protected override TMessage? Serialize(IFieldUser user, IPacketReader reader)
    {
        var dragon = user.Owned
            .OfType<IFieldDragon>()
            .FirstOrDefault();

        if (dragon == null) return default;
        if (dragon.Owner != user) return default;

        return Serialize(user, dragon, reader);
    }

    protected abstract TMessage? Serialize(IFieldUser user, IFieldDragon dragon, IPacketReader reader);
}
