using System.Linq;
using System.Threading.Tasks;
using Edelstein.Common.Gameplay.Game.Conversations;
using Edelstein.Common.Gameplay.Game.Conversations.Speakers;
using Edelstein.Protocol.Gameplay.Game.Contracts.Packets.Recv;
using Edelstein.Protocol.Gameplay.Game.Conversations;
using Edelstein.Protocol.Gameplay.Game.Conversations.Speakers;
using Edelstein.Protocol.Gameplay.Game.Objects;
using Edelstein.Protocol.Gameplay.Game.Objects.NPC;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay.Game.Handling.Pipes;

public class UserOnPacketUserSelectNPC(
    IConversationManager<IConversationSpeakerNPC, IConversationSpeakerUser> conversations
) : AbstractUserOnPacketInField<UserSelectNPC>
{
    protected override async Task HandleAfter(IPipelineContext ctx, PipedFieldPacketMessage<UserSelectNPC> message)
    {
        var obj = message.User.Field?
            .GetPool(FieldObjectType.NPC)?
            .GetObject(message.Packet.ObjectID);

        if (obj is not IFieldNPC npc) return;
        if (npc.FieldSplit != null && !message.User.Observing.Contains(npc.FieldSplit)) return;

        var script = npc.Template.Scripts.FirstOrDefault()?.Script;
        if (script == null) return;
        var conversation = (IConversation<IConversationSpeakerNPC, IConversationSpeakerUser>?)await conversations.Retrieve(script) ??
                            new SystemConversationFallback<IConversationSpeakerNPC, IConversationSpeakerUser>(
                                script,
                                message.User
                            );

        _ = message.User.Converse(
            conversation,
            c => new ConversationSpeakerNPC(c, npc),
            c => new ConversationSpeakerUser(c, message.User)
        );
    }
}
