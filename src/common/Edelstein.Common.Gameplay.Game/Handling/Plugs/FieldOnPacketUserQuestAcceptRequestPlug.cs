using Edelstein.Common.Gameplay.Handling;
using Edelstein.Common.Utilities.Packets;
using Edelstein.Protocol.Gameplay.Game.Contracts;
using Edelstein.Protocol.Gameplay.Game.Quests;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay.Game.Handling.Plugs;

public class FieldOnPacketUserQuestAcceptRequestPlug : IPipelinePlug<FieldOnPacketUserQuestAcceptRequest>
{
    private readonly IQuestManager _manager;
    
    public FieldOnPacketUserQuestAcceptRequestPlug(IQuestManager manager) => _manager = manager;
    
    public async Task Handle(IPipelineContext ctx, FieldOnPacketUserQuestAcceptRequest message)
    {
        if (message.Template.CheckStart.ScriptStart != null) return;

        var result = await _manager.Accept(message.User, message.Template.ID);
        using var packet = new PacketWriter(PacketSendOperations.UserQuestResult);
        
        packet.WriteByte((byte)result);
        switch (result)
        {
            case QuestResultType.FailedInventory:
            case QuestResultType.ResetQuestTimer:
            case QuestResultType.FailedTimeOver:
                packet.WriteShort((short)message.Template.ID);
                break;
            case QuestResultType.Success:
                packet.WriteShort((short)message.Template.ID);
                packet.WriteInt(message.NPCTemplateID ?? 0);
                packet.WriteShort((short)(message.Template.ActStart.NextQuest ?? 0));
                break;
        }
        
        await message.User.Dispatch(packet.Build());
    }
}
