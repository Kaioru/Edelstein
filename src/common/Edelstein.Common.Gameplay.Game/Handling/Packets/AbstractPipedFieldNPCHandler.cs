using Edelstein.Protocol.Gameplay.Game.Objects;
using Edelstein.Protocol.Gameplay.Game.Objects.NPC;
using Edelstein.Protocol.Gameplay.Game.Objects.User;
using Edelstein.Protocol.Utilities.Packets;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay.Game.Handling.Packets;

public abstract class AbstractPipedFieldNPCHandler<TMessage> : AbstractPipedFieldHandler<TMessage>
{
    protected AbstractPipedFieldNPCHandler(IPipeline<TMessage> pipeline) : base(pipeline)
    {
    }

    protected override TMessage? Serialize(IFieldUser user, IPacketReader reader)
    {
        var objID = reader.ReadInt();
        var obj = user.Field?.GetPool(FieldObjectType.NPC)?.GetObject(objID);

        if (obj is not IFieldNPC npc) return default;
        if (npc.Controller != user) return default;

        return Serialize(user, npc, reader);
    }

    protected abstract TMessage? Serialize(IFieldUser user, IFieldNPC npc, IPacketReader reader);
}
