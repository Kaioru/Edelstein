using Edelstein.Common.Gameplay.Game.Conversations.Speakers;
using Edelstein.Protocol.Gameplay.Game.Contracts;
using Edelstein.Protocol.Gameplay.Game.Conversations;
using Edelstein.Protocol.Gameplay.Game.Conversations.Speakers;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay.Game.Plugs;

public class FieldOnPacketUserSelectNPCPlug : IPipelinePlug<FieldOnPacketUserSelectNPC>
{
    private readonly IConversationManager _manager;
    
    public FieldOnPacketUserSelectNPCPlug(IConversationManager manager) => _manager = manager;

    public async Task Handle(IPipelineContext ctx, FieldOnPacketUserSelectNPC message)
    {
        var script = message.NPC.Template.Scripts.FirstOrDefault()?.Script;
        if (script == null) return;
        var conversation = await _manager.Create(script);

        _ = message.User.Converse(
            conversation,
            c => new ConversationSpeaker(c, message.NPC.Template.ID),
            c => new ConversationSpeaker(c, flags: ConversationSpeakerFlags.NPCReplacedByUser)
        );
    }
}
