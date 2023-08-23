using Edelstein.Protocol.Gameplay.Game.Objects;
using Edelstein.Protocol.Gameplay.Game.Objects.NPC;
using Edelstein.Protocol.Gameplay.Game.Objects.User;
using Edelstein.Protocol.Utilities.Packets;

namespace Edelstein.Common.Gameplay.Game.Handlers;

public abstract class AbstractFieldNPCHandler : AbstractFieldHandler
{
    protected override async Task Handle(IFieldUser user, IPacketReader reader)
    {
        var objID = reader.ReadInt();
        var obj = user.Field?.GetPool(FieldObjectType.NPC)?.GetObject(objID);

        if (obj is not IFieldNPC npc) return;
        if (npc.Controller != user) return;

        await Handle(user, npc, reader);
    }

    protected abstract Task Handle(IFieldUser user, IFieldNPC npc, IPacketReader reader);
}
