﻿using Edelstein.Protocol.Gameplay.Game.Combat;
using Edelstein.Protocol.Gameplay.Game.Contracts;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay.Game.Handling.Plugs;

public class FieldOnPacketSummonedSkillPlug : IPipelinePlug<FieldOnPacketSummonedSkill>
{
    private readonly ISkillManager _skillManager;

    public FieldOnPacketSummonedSkillPlug(ISkillManager skillManager) 
        => _skillManager = skillManager;

    public async Task Handle(IPipelineContext ctx, FieldOnPacketSummonedSkill message)
    {
        if (await _skillManager.Check(message.User, message.SkillID))
            await _skillManager.HandleSkillUse(message.User, message.SkillID);
    }
}
