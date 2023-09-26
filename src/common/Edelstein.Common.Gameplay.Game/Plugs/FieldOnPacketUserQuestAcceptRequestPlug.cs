using Edelstein.Common.Gameplay.Game.Objects.User.Messages;
using Edelstein.Common.Gameplay.Models.Characters.Quests;
using Edelstein.Common.Gameplay.Packets;
using Edelstein.Common.Utilities.Packets;
using Edelstein.Protocol.Gameplay.Game.Contracts;
using Edelstein.Protocol.Gameplay.Game.Quests;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay.Game.Plugs;

public class FieldOnPacketUserQuestAcceptRequestPlug : IPipelinePlug<FieldOnPacketUserQuestAcceptRequest>
{
    private readonly IQuestManager _manager;
    
    public FieldOnPacketUserQuestAcceptRequestPlug(IQuestManager manager) => _manager = manager;
    
    public async Task Handle(IPipelineContext ctx, FieldOnPacketUserQuestAcceptRequest message)
    {
        var result = await _manager.Check(
            QuestAction.Start, 
            message.Template, 
            message.User
        );

        if (message.Template.CheckStart.ScriptStart != null) return;

        if (result == QuestResultType.Success)
            result = await _manager.Act(
                QuestAction.Start,
                message.Template,
                message.User,
                null
            );

        if (result == QuestResultType.Success)
            await _manager.Accept(message.User, message.Template.ID);

        var p = new PacketWriter(PacketSendOperations.UserQuestResult);
        
        p.WriteByte((byte)result);
        switch (result)
        {
            case QuestResultType.FailedInventory:
            case QuestResultType.ResetQuestTimer:
            case QuestResultType.FailedTimeOver:
                p.WriteShort((short)message.Template.ID);
                break;
            case QuestResultType.Success:
                p.WriteShort((short)message.Template.ID);
                p.WriteInt(message.NPCTemplateID ?? 0);
                p.WriteShort((short)(message.Template.ActStart.NextQuest ?? 0));
                break;
        }
        
        await message.User.Dispatch(p.Build());
    }
}
