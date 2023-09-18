using Edelstein.Common.Gameplay.Packets;
using Edelstein.Protocol.Gameplay.Game.Contracts;
using Edelstein.Protocol.Gameplay.Game.Objects;
using Edelstein.Protocol.Gameplay.Game.Objects.NPC;
using Edelstein.Protocol.Gameplay.Game.Objects.User;
using Edelstein.Protocol.Utilities.Packets;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay.Game.Handlers;

public class UserSelectNPCHandler : AbstractPipedFieldHandler<FieldOnPacketUserSelectNPC>
{

    public UserSelectNPCHandler(IPipeline<FieldOnPacketUserSelectNPC> pipeline) : base(pipeline)
    {
    }

    public override short Operation => (short)PacketRecvOperations.UserSelectNpc;

    protected override FieldOnPacketUserSelectNPC? Serialize(IFieldUser user, IPacketReader reader)
    {
        
        var objID = reader.ReadInt();
        var obj = user.Field?.GetPool(FieldObjectType.NPC)?.GetObject(objID);

        if (obj is not IFieldNPC npc) return default;
        if (npc.FieldSplit != null && !user.Observing.Contains(npc.FieldSplit)) return default;

        return new(user, npc);
    }
}
