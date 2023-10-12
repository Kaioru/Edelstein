using Edelstein.Common.Gameplay.Game.Conversations;
using Edelstein.Common.Gameplay.Game.Conversations.Speakers;
using Edelstein.Common.Gameplay.Game.Dialogues;
using Edelstein.Protocol.Gameplay.Game.Contracts;
using Edelstein.Protocol.Gameplay.Game.Conversations;
using Edelstein.Protocol.Gameplay.Game.Conversations.Speakers;
using Edelstein.Protocol.Gameplay.Game.Objects.NPC;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay.Game.Handling.Plugs;

public class FieldOnPacketUserSelectNPCPlug : IPipelinePlug<FieldOnPacketUserSelectNPC>
{
    private readonly INamedConversationManager _conversationManager;
    private readonly INPCShopManager _shopManager;
    
    public FieldOnPacketUserSelectNPCPlug(INamedConversationManager conversationManager, INPCShopManager shopManager)
    {
        _conversationManager = conversationManager;
        _shopManager = shopManager;
    }

    public async Task Handle(IPipelineContext ctx, FieldOnPacketUserSelectNPC message)
    {
        var shop = await _shopManager.Retrieve(message.NPC.Template.ID);

        if (shop != null)
        {
            await message.User.Dialogue(new DialogueNPCShop(shop));
            return;
        }
        
        var script = message.NPC.Template.Scripts.FirstOrDefault()?.Script;
        if (script == null) return;
        var conversation = await _conversationManager.Retrieve(script) as IConversation ?? 
                           new FallbackConversation(script, message.User);

        _ = message.User.Converse(
            conversation,
            c => new ConversationSpeakerNPC(message.NPC, c, message.NPC.Template.ID),
            c => new ConversationSpeakerUser(message.User, c, flags: ConversationSpeakerFlags.NPCReplacedByUser)
        );
    }
}
