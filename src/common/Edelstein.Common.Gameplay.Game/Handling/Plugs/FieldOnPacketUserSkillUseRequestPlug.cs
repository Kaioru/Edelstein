using Edelstein.Common.Gameplay.Handling;
using Edelstein.Common.Utilities.Packets;
using Edelstein.Protocol.Gameplay.Game.Combat;
using Edelstein.Protocol.Gameplay.Game.Contracts;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay.Game.Handling.Plugs;

public class FieldOnPacketUserSkillUseRequestPlug : IPipelinePlug<FieldOnPacketUserSkillUseRequest>
{
    private readonly ISkillManager _skillManager;

    public FieldOnPacketUserSkillUseRequestPlug(ISkillManager skillManager) 
        => _skillManager = skillManager;
    
    public async Task Handle(IPipelineContext ctx, FieldOnPacketUserSkillUseRequest message)
    {
        if (!await _skillManager.Check(message.User, message.SkillID)) 
            return;

        await _skillManager.HandleSkillUse(message.User, message.SkillID);
        
        await message.User.Dispatch(new PacketWriter(PacketSendOperations.SkillUseResult)
            .WriteBool(true)
            .Build());
    }
}
