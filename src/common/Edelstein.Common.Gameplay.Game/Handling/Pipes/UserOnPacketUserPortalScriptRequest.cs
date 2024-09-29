using System.Linq;
using System.Threading.Tasks;
using Edelstein.Common.Gameplay.Game.Conversations;
using Edelstein.Common.Gameplay.Game.Conversations.Speakers;
using Edelstein.Protocol.Gameplay.Game.Contracts.Packets.Recv;
using Edelstein.Protocol.Gameplay.Game.Conversations;
using Edelstein.Protocol.Gameplay.Game.Conversations.Speakers;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay.Game.Handling.Pipes;

public class UserOnPacketUserPortalScriptRequest(
    IConversationManager<IConversationSpeakerPortal, IConversationSpeakerUser> conversations
) : AbstractUserOnPacketInField<UserPortalScriptRequest>
{
    protected override async Task HandleAfter(IPipelineContext ctx, PipedFieldPacketMessage<UserPortalScriptRequest> message)
    {
        if (message.User.Field == null) return;
        
        var portal = (await message.User.Field.Template.Portals.RetrieveAll())
            .FirstOrDefault(p => p.Name == message.Packet.Name.Value);
        var script = portal?.Script;
        if (script == null) return;
        var conversation = (IConversation<IConversationSpeakerPortal, IConversationSpeakerUser>?)await conversations.Retrieve(script) ??
                           new SystemConversationFallback<IConversationSpeakerPortal, IConversationSpeakerUser>(
                               script,
                               message.User
                           );

        _ = message.User.Converse(
            conversation,
            c => new ConversationSpeakerPortal(c),
            c => new ConversationSpeakerUser(c, message.User)
        );
    }
}
