using Edelstein.Protocol.Gameplay.Game.Combat;
using Edelstein.Protocol.Gameplay.Game.Contracts;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay.Game.Plugs;

public class FieldOnPacketUserSkillCancelRequestPlug : IPipelinePlug<FieldOnPacketUserSkillCancelRequest>
{
    private readonly ISkillManager _skillManager;
    
    public FieldOnPacketUserSkillCancelRequestPlug(ISkillManager skillManager) => _skillManager = skillManager;
    
    public async Task Handle(IPipelineContext ctx, FieldOnPacketUserSkillCancelRequest message)
    {
        await _skillManager.HandleSkillCancel(message.User, message.SkillID);
        await message.User.ModifyStats(exclRequest: true);
    }
}
