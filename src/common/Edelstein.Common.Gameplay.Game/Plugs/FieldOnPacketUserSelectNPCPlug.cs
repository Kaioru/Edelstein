using Edelstein.Common.Gameplay.Game.Conversations;
using Edelstein.Common.Gameplay.Game.Conversations.Speakers;
using Edelstein.Protocol.Gameplay.Game.Contracts;
using Edelstein.Protocol.Gameplay.Game.Conversations;
using Edelstein.Protocol.Gameplay.Game.Conversations.Speakers;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay.Game.Plugs;

public class FieldOnPacketUserSelectNPCPlug : IPipelinePlug<FieldOnPacketUserSelectNPC>
{
    private readonly INamedConversationManager _manager;
    
    public FieldOnPacketUserSelectNPCPlug(INamedConversationManager manager) => _manager = manager;

    public async Task Handle(IPipelineContext ctx, FieldOnPacketUserSelectNPC message)
    {
        var script = message.NPC.Template.Scripts.FirstOrDefault()?.Script;
        if (script == null) return;
        var conversation = await _manager.Retrieve(script) as IConversation ?? 
                           new FallbackConversation(script, message.User);

        _ = message.User.Converse(
            conversation,
            c => new ConversationSpeakerNPC(message.NPC, c, message.NPC.Template.ID),
            c => new ConversationSpeakerUser(message.User, c, flags: ConversationSpeakerFlags.NPCReplacedByUser)
        );
    }
}
