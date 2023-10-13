using Edelstein.Common.Gameplay.Handling;
using Edelstein.Common.Utilities.Packets;
using Edelstein.Protocol.Gameplay.Game.Combat;
using Edelstein.Protocol.Gameplay.Game.Contracts;
using Edelstein.Protocol.Gameplay.Models.Characters.Skills.Templates;
using Edelstein.Protocol.Utilities.Pipelines;
using Edelstein.Protocol.Utilities.Templates;

namespace Edelstein.Common.Gameplay.Game.Handling.Plugs;

public class FieldOnPacketUserSkillCancelRequestPlug : IPipelinePlug<FieldOnPacketUserSkillCancelRequest>
{
    private readonly ISkillManager _skillManager;
    private readonly ITemplateManager<ISkillTemplate> _skillTemplates;

    public FieldOnPacketUserSkillCancelRequestPlug(
        ISkillManager skillManager, 
        ITemplateManager<ISkillTemplate> skillTemplates
    )
    {
        _skillManager = skillManager;
        _skillTemplates = skillTemplates;
    }

    public async Task Handle(IPipelineContext ctx, FieldOnPacketUserSkillCancelRequest message)
    {
        if (message.SkillID > 0)
        {
            var skill = await _skillTemplates.Retrieve(message.SkillID);

            if (skill?.IsPrepared ?? false)
            {
                using var packet = new PacketWriter(PacketSendOperations.UserSkillCancel);

                packet.WriteInt(message.User.Character.ID);
                packet.WriteInt(message.SkillID);

                if (message.User.FieldSplit != null)
                    await message.User.FieldSplit.Dispatch(packet.Build());
            }
        }
        
        await _skillManager.HandleSkillCancel(message.User, message.SkillID);
        await message.User.ModifyStats(exclRequest: true);
    }
}
