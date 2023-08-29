using Edelstein.Common.Gameplay.Constants;
using Edelstein.Protocol.Gameplay.Game.Contracts;
using Edelstein.Protocol.Gameplay.Models.Characters.Skills.Templates;
using Edelstein.Protocol.Utilities.Pipelines;
using Edelstein.Protocol.Utilities.Templates;

namespace Edelstein.Common.Gameplay.Game.Plugs;

public class FieldOnPacketUserSkillUpRequestPlug : IPipelinePlug<FieldOnPacketUserSkillUpRequest>
{
    private readonly ITemplateManager<ISkillTemplate> _templates;
    
    public FieldOnPacketUserSkillUpRequestPlug(ITemplateManager<ISkillTemplate> templates) => _templates = templates;
    
    public async Task Handle(IPipelineContext ctx, FieldOnPacketUserSkillUpRequest message)
    {
        var template = await _templates.Retrieve(message.templateID);
        const short increment = 1;

        if (template == null) return;
        if (template.IsInvisible) return;
        if (template.ReqSkill
            .Any(kv => (message.User.Character.Skills[kv.Key]?.Level ?? 0) < kv.Value))
            return;
        
        int maxLevel = template.MaxLevel;
        
        if (SkillConstants.IsSkillNeedMasterLevel(template.ID))
            maxLevel = message.User.Character.Skills[template.ID]?.MasterLevel ?? 0;
        
        if ((message.User.Character.Skills[template.ID]?.Level ?? 0) + increment > maxLevel) return;

        var job = message.User.Character.Job;
        var skillJob = template.ID / 10000;
        var skillJobLevel = (byte)JobConstants.GetJobLevel(skillJob);
        
        if (skillJobLevel == 0)
        {
            var sp = Math.Min(message.User.Character.Level - 1, job == Job.Citizen ? 9 : 6);
            
            if (JobConstants.GetBeginnerJob(job) != skillJob) return;
            
            for (var i = 0; i < 3; i++) 
                sp -= message.User.Character.Skills[job * 1000 + 1000 + i]?.Level ?? 0;

            if (sp < increment) return;

            await message.User.ModifySkills(s => s.Add(template.ID));
            return;
        }

        if (JobConstants.GetJobRace(job) != JobConstants.GetJobRace(skillJob) ||
            JobConstants.GetJobType(job) != JobConstants.GetJobType(skillJob) ||
            JobConstants.GetJobLevel(job) < JobConstants.GetJobLevel(skillJob)) 
            return;
        
        if (JobConstants.IsExtendSPJob(job) && message.User.Character.ExtendSP[(byte)skillJobLevel] < increment) return;
        if (!JobConstants.IsExtendSPJob(job) && message.User.Character.SP < increment) return;

        await message.User.ModifyStats(s =>
        {
            if (JobConstants.IsExtendSPJob(job))
                s.SetExtendSP(
                    skillJobLevel, 
                    (byte)((message.User.Character.ExtendSP[skillJobLevel] ?? 0) - increment)
                );
            else 
                s.SP -= increment;
        });
        await message.User.ModifySkills(s => s.Add(template.ID, increment), true);
    }
}
