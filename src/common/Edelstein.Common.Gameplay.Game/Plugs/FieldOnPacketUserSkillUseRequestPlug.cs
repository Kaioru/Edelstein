﻿using Edelstein.Common.Gameplay.Packets;
using Edelstein.Common.Utilities.Packets;
using Edelstein.Protocol.Gameplay.Game.Combat;
using Edelstein.Protocol.Gameplay.Game.Contracts;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay.Game.Plugs;

public class FieldOnPacketUserSkillUseRequestPlug : IPipelinePlug<FieldOnPacketUserSkillUseRequest>
{
    private readonly ISkillManager _skillManager;

    public FieldOnPacketUserSkillUseRequestPlug(ISkillManager skillManager) 
        => _skillManager = skillManager;
    
    public async Task Handle(IPipelineContext ctx, FieldOnPacketUserSkillUseRequest message)
    {
        if (!await _skillManager.ProcessUserSkill(message.User, message.SkillID)) return;
        
        await message.User.Dispatch(new PacketWriter(PacketSendOperations.SkillUseResult)
            .WriteBool(true)
            .Build());
    }
}
