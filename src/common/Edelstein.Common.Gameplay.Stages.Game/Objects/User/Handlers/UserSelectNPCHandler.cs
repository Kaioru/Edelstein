using Edelstein.Common.Gameplay.Packets;
using Edelstein.Common.Gameplay.Stages.Game.Conversations.Speakers;
using Edelstein.Protocol.Gameplay.Stages.Game.Conversations;
using Edelstein.Protocol.Gameplay.Stages.Game.Conversations.Speakers;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.NPC;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.User;
using Edelstein.Protocol.Util.Buffers.Packets;

namespace Edelstein.Common.Gameplay.Stages.Game.Objects.User.Handlers;

public class UserSelectNPCHandler : AbstractFieldUserHandler
{
    private readonly IConversationManager _manager;

    public UserSelectNPCHandler(IConversationManager manager) => _manager = manager;

    public override short Operation => (short)PacketRecvOperations.UserSelectNPC;

    protected override async Task Handle(IFieldUser user, IPacketReader reader)
    {
        var objID = reader.ReadInt();
        var obj = user.Field?.GetPool(FieldObjectType.NPC)?.GetObject(objID);

        if (obj is not IFieldNPC npc) return;
        if (npc.FieldSplit != null && !user.Observing.Contains(npc.FieldSplit)) return;

        var script = npc.Template.Scripts.FirstOrDefault()?.Script;
        if (script == null) return;
        var conversation = await _manager.Create(script);

        _ = user.Converse(
            conversation,
            c => new ConversationSpeaker(c, npc.Template.ID),
            c => new ConversationSpeaker(c, flags: ConversationSpeakerFlags.NPCReplacedByUser)
        );
    }
}
