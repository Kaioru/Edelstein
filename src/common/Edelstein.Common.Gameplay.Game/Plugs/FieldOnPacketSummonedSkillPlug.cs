using Edelstein.Protocol.Gameplay.Game.Combat;
using Edelstein.Protocol.Gameplay.Game.Contracts;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay.Game.Plugs;

public class FieldOnPacketSummonedSkillPlug : IPipelinePlug<FieldOnPacketSummonedSkill>
{
    private readonly ISkillManager _skillManager;

    public FieldOnPacketSummonedSkillPlug(ISkillManager skillManager) 
        => _skillManager = skillManager;
    
    public async Task Handle(IPipelineContext ctx, FieldOnPacketSummonedSkill message)
    {
        if (!await _skillManager.ProcessUserSkill(message.User, message.SkillID)) return;
        // TODO
    }
}
