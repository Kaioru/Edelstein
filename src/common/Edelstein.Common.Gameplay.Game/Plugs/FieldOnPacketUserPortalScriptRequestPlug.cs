using Edelstein.Common.Gameplay.Game.Conversations;
using Edelstein.Common.Gameplay.Game.Conversations.Speakers;
using Edelstein.Protocol.Gameplay.Game.Contracts;
using Edelstein.Protocol.Gameplay.Game.Conversations;
using Edelstein.Protocol.Gameplay.Game.Conversations.Speakers;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay.Game.Plugs;

public class FieldOnPacketUserPortalScriptRequestPlug : IPipelinePlug<FieldOnPacketUserPortalScriptRequest>
{
    private readonly INamedConversationManager _scriptManager;

    public FieldOnPacketUserPortalScriptRequestPlug(INamedConversationManager scriptManager) => _scriptManager = scriptManager;
    
    public async Task Handle(IPipelineContext ctx, FieldOnPacketUserPortalScriptRequest message)
    {
        var portal = message.User.Field?.Template.Portals.Objects.FirstOrDefault(p => p.Name == message.Portal);
        if (portal?.Script == null) return;
        
        var conversation = await _scriptManager.Retrieve(portal.Script) as IConversation ?? 
                           new FallbackConversation(portal.Script, message.User);

        _ = message.User.Converse(
            conversation,
            c => new ConversationSpeaker(c),
            c => new ConversationSpeaker(c, flags: ConversationSpeakerFlags.NPCReplacedByUser)
        );
    }
}
