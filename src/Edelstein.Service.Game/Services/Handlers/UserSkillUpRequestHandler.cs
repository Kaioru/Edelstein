using System;
using System.Threading.Tasks;
using Edelstein.Core;
using Edelstein.Core.Extensions;
using Edelstein.Core.Gameplay.Constants;
using Edelstein.Network.Packets;
using Edelstein.Provider.Templates.Skill;
using Edelstein.Service.Game.Fields.Objects.User;

namespace Edelstein.Service.Game.Services.Handlers
{
    public class UserSkillUpRequestHandler : AbstractFieldUserHandler
    {
        public override async Task Handle(RecvPacketOperations operation, IPacket packet, FieldUser user)
        {
            packet.Decode<int>();
            var templateID = packet.Decode<int>();
            var template = user.Service.TemplateManager.Get<SkillTemplate>(templateID);

            Console.WriteLine(template);
            
            if (template == null) return;

            var job = template.ID / 10000;
            var jobLevel = (byte) SkillConstants.GetJobLevel(job);

            if (jobLevel == 0)
            {
                var sp = Math.Min(user.Character.Level - 1, job == 3000 ? 9 : 6);
                for (var i = 0; i < 3; i++)
                    sp -= user.Character.GetSkillLevel(job * 1000 + 1000 + i);
                if (sp > 0)
                    await user.ModifySkills(s => s.Add(templateID), true);
                return;
            }

            if (SkillConstants.IsExtendSPJob(job) && user.Character.GetExtendSP(jobLevel) <= 0) return;
            if (!SkillConstants.IsExtendSPJob(job) && user.Character.SP <= 0) return;

            var maxLevel = template.MaxLevel;
            
            if (SkillConstants.IsSkillNeedMasterLevel(templateID))
                maxLevel = (short) user.Character.GetSkillMasterLevel(templateID);

            if (user.Character.GetSkillLevel(templateID) >= maxLevel) return;

            await user.ModifyStats(s =>
            {
                if (SkillConstants.IsExtendSPJob(job))
                    s.SetExtendSP(jobLevel, (byte) (s.GetExtendSP(jobLevel) - 1));
                else s.SP--;
            });
            await user.ModifySkills(s => s.Add(templateID), true);
        }
    }
}