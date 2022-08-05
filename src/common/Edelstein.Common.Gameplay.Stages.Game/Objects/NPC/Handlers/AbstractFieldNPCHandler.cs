using Edelstein.Common.Gameplay.Stages.Game.Objects.User.Handlers;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.NPC;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.User;
using Edelstein.Protocol.Util.Buffers.Packets;

namespace Edelstein.Common.Gameplay.Stages.Game.Objects.NPC.Handlers;

public abstract class AbstractFieldNPCHandler : AbstractFieldUserHandler
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
