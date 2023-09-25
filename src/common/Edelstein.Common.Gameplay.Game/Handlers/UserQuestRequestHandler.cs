using Edelstein.Common.Gameplay.Game.Quests;
using Edelstein.Common.Gameplay.Packets;
using Edelstein.Common.Utilities.Packets;
using Edelstein.Protocol.Gameplay.Game.Contracts;
using Edelstein.Protocol.Gameplay.Game.Objects.User;
using Edelstein.Protocol.Gameplay.Game.Quests;
using Edelstein.Protocol.Gameplay.Game.Quests.Templates;
using Edelstein.Protocol.Utilities.Packets;
using Edelstein.Protocol.Utilities.Spatial;
using Edelstein.Protocol.Utilities.Templates;
using Microsoft.Extensions.Logging;

namespace Edelstein.Common.Gameplay.Game.Handlers;

public class UserQuestRequestHandler : AbstractFieldHandler 
{
    private readonly ILogger _logger;
    private readonly ITemplateManager<IQuestTemplate> _templates;

    public UserQuestRequestHandler(ILogger<UserQuestRequestHandler> logger, ITemplateManager<IQuestTemplate> templates)
    {
        _logger = logger;
        _templates = templates;
    }

    public override short Operation => (short)PacketRecvOperations.UserQuestRequest;
        
    protected override async Task Handle(IFieldUser user, IPacketReader reader)
    {
        var type = (QuestRequestType)reader.ReadByte();
        var questID = reader.ReadShort();
        var quest = await _templates.Retrieve(questID);

        if (quest == null) return;

        int? npcID = null;
        IPoint2D? userPosition = null;

        if (type != QuestRequestType.LostItem && type != QuestRequestType.ResignQuest)
        {
            npcID = reader.ReadInt();

            if (!quest.IsAutoStart)
                userPosition = reader.ReadPoint2D();
        }

        switch (type)
        {
            case QuestRequestType.LostItem:
                var lostItemCount = reader.ReadInt();
                var lostItem = new List<int>();
                
                for (var i = 0; i < lostItemCount; i++)
                    lostItem.Add(reader.ReadInt());
                
                await user.StageUser.Context.Pipelines.FieldOnPacketUserQuestLostItemRequest.Process(new FieldOnPacketUserQuestLostItemRequest(
                    user,
                    quest,
                    lostItem
                ));
                break;
            case QuestRequestType.AcceptQuest:
                await user.StageUser.Context.Pipelines.FieldOnPacketUserQuestAcceptRequest.Process(new FieldOnPacketUserQuestAcceptRequest(
                    user,
                    quest,
                    npcID,
                    userPosition
                ));
                break;
            case QuestRequestType.CompleteQuest:
                await user.StageUser.Context.Pipelines.FieldOnPacketUserQuestCompleteRequest.Process(new FieldOnPacketUserQuestCompleteRequest(
                    user,
                    quest,
                    npcID,
                    userPosition,
                    !quest.IsAutoComplete ? reader.ReadInt() : null
                ));
                break;
            case QuestRequestType.ResignQuest:
                await user.StageUser.Context.Pipelines.FieldOnPacketUserQuestResignRequest.Process(new FieldOnPacketUserQuestResignRequest(
                    user,
                    quest
                ));
                break;
            case QuestRequestType.OpeningScript:
                await user.StageUser.Context.Pipelines.FieldOnPacketUserQuestScriptStartRequest.Process(new FieldOnPacketUserQuestScriptStartRequest(
                    user,
                    quest,
                    npcID,
                    userPosition
                ));
                break;
            case QuestRequestType.CompleteScript:
                await user.StageUser.Context.Pipelines.FieldOnPacketUserQuestScriptEndRequest.Process(new FieldOnPacketUserQuestScriptEndRequest(
                    user,
                    quest,
                    npcID,
                    userPosition
                ));
                break;
            default:
                _logger.LogWarning("Unhandled quest request type {Type}", type);
                break;
        }
    }
}
