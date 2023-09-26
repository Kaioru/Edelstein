﻿using Edelstein.Common.Gameplay.Game.Conversations;
using Edelstein.Common.Gameplay.Game.Conversations.Speakers;
using Edelstein.Protocol.Gameplay.Game.Contracts;
using Edelstein.Protocol.Gameplay.Game.Conversations;
using Edelstein.Protocol.Gameplay.Game.Conversations.Speakers;
using Edelstein.Protocol.Gameplay.Game.Quests;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay.Game.Plugs;

public class FieldOnPacketUserQuestScriptStartRequestPlug : IPipelinePlug<FieldOnPacketUserQuestScriptStartRequest>
{
    private readonly IQuestManager _manager;

    public FieldOnPacketUserQuestScriptStartRequestPlug(IQuestManager manager) => _manager = manager;

    public Task Handle(IPipelineContext ctx, FieldOnPacketUserQuestScriptStartRequest message) 
        => _manager.Script(QuestAction.Start, message.User, message.Template.ID, message.NPCTemplateID);
}
