using Edelstein.Common.Gameplay.Game.Conversations;
using Edelstein.Common.Gameplay.Game.Conversations.Speakers;
using Edelstein.Protocol.Gameplay.Game.Contracts;
using Edelstein.Protocol.Gameplay.Game.Conversations;
using Edelstein.Protocol.Gameplay.Game.Conversations.Speakers;
using Edelstein.Protocol.Gameplay.Game.Quests;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay.Game.Plugs;

public class FieldOnPacketUserQuestScriptEndRequestPlug : IPipelinePlug<FieldOnPacketUserQuestScriptEndRequest>
{
    private readonly IQuestManager _manager;
    private readonly INamedConversationManager _scriptManager;

    public FieldOnPacketUserQuestScriptEndRequestPlug(IQuestManager manager, INamedConversationManager scriptManager)
    {
        _manager = manager;
        _scriptManager = scriptManager;
    }

    public async Task Handle(IPipelineContext ctx, FieldOnPacketUserQuestScriptEndRequest message)
    {
        if (message.Template.CheckEnd.ScriptEnd == null) return;
        
        var result = await _manager.Accept(message.User, message.Template.ID);

        if (result != QuestResultType.Success) return;
        
        var conversation = await _scriptManager.Retrieve(message.Template.CheckEnd.ScriptEnd) as IConversation ?? 
                           new FallbackConversation(message.Template.CheckEnd.ScriptEnd, message.User);

        _ = message.User.Converse(
            conversation,
            c => new ConversationSpeaker(c, message.NPCTemplateID ?? 9010000),
            c => new ConversationSpeakerUser(message.User, c, flags: ConversationSpeakerFlags.NPCReplacedByUser)
        );
    }
}
