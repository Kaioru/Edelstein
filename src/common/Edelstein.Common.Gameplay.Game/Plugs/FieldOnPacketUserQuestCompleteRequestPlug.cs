using Edelstein.Common.Gameplay.Game.Objects.User.Effects;
using Edelstein.Common.Gameplay.Game.Objects.User.Messages;
using Edelstein.Common.Gameplay.Models.Characters.Quests;
using Edelstein.Common.Gameplay.Packets;
using Edelstein.Common.Utilities.Packets;
using Edelstein.Protocol.Gameplay.Game.Contracts;
using Edelstein.Protocol.Gameplay.Game.Conversations;
using Edelstein.Protocol.Gameplay.Game.Quests;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay.Game.Plugs;

public class FieldOnPacketUserQuestCompleteRequestPlug : IPipelinePlug<FieldOnPacketUserQuestCompleteRequest>
{
    private readonly IQuestManager _manager;
    
    public FieldOnPacketUserQuestCompleteRequestPlug(IQuestManager manager) => _manager = manager;

    public async Task Handle(IPipelineContext ctx, FieldOnPacketUserQuestCompleteRequest message)
    {
        if (message.Template.CheckEnd.ScriptEnd != null) return;
       
        var result = await _manager.Complete(message.User, message.Template.ID);
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
                p.WriteShort((short)(message.Template.ActEnd.NextQuest ?? 0));
                break;
        }
        
        await message.User.Dispatch(p.Build());
    }
}
