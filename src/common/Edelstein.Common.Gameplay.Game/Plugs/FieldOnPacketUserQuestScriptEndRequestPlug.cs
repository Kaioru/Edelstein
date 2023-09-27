using Edelstein.Protocol.Gameplay.Game.Contracts;
using Edelstein.Protocol.Gameplay.Game.Quests;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay.Game.Plugs;

public class FieldOnPacketUserQuestScriptEndRequestPlug : IPipelinePlug<FieldOnPacketUserQuestScriptEndRequest>
{
    private readonly IQuestManager _manager;

    public FieldOnPacketUserQuestScriptEndRequestPlug(IQuestManager manager) => _manager = manager;

    public Task Handle(IPipelineContext ctx, FieldOnPacketUserQuestScriptEndRequest message)
        => _manager.Script(QuestAction.End, message.User, message.Template.ID, message.NPCTemplateID);
}
