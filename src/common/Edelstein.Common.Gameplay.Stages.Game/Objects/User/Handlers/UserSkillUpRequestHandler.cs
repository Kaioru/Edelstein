using System;
using System.Threading.Tasks;
using Edelstein.Common.Gameplay.Constants;
using Edelstein.Common.Gameplay.Handling;
using Edelstein.Common.Gameplay.Users;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.User;
using Edelstein.Protocol.Network;

namespace Edelstein.Common.Gameplay.Stages.Game.Objects.User.Handlers
{
    public class UserSkillUpRequestHandler : AbstractUserPacketHandler
    {
        public override short Operation => (short)PacketRecvOperations.UserSkillUpRequest;

        protected override async Task Handle(GameStageUser stageUser, IFieldObjUser user, IPacketReader packet)
        {
            _ = packet.ReadInt();
            var templateID = packet.ReadInt();
            var template = await stageUser.Stage.SkillTemplates.Retrieve(templateID);

            if (template == null) return;

            var job = template.ID / 10000;
            var jobLevel = (byte)GameConstants.GetJobLevel(job);

            // TODO: job checks

            if (jobLevel == 0)
            {
                var sp = Math.Min(user.Character.Level - 1, job == 3000 ? 9 : 6);

                for (var i = 0; i < 3; i++) sp -= user.Character.GetSkillLevel(job * 1000 + 1000 + i);
                if (sp > 0) await user.ModifySkills(s => s.Add(templateID), true);
                return;
            }

            if (GameConstants.IsExtendSPJob(job) && user.Character.GetExtendSP(jobLevel) <= 0) return;
            if (!GameConstants.IsExtendSPJob(job) && user.Character.SP <= 0) return;

            var maxLevel = template.MaxLevel;

            if (GameConstants.IsSkillNeedMasterLevel(templateID))
                maxLevel = (short)user.Character.GetSkillMasterLevel(templateID);

            if (user.Character.GetSkillLevel(templateID) >= maxLevel) return;

            var increment = 1;

            await user.ModifyStats(s =>
            {
                if (GameConstants.IsExtendSPJob(job))
                    s.SetExtendSP(jobLevel, (byte)(s.GetExtendSP(jobLevel) - increment));
                else s.SP--;
            });
            await user.ModifySkills(s => s.Add(templateID, increment), true);
        }
    }
}
