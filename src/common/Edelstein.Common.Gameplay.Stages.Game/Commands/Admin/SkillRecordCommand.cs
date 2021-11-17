using System;
using System.Threading.Tasks;
using Edelstein.Common.Gameplay.Constants;
using Edelstein.Common.Gameplay.Constants.Types;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.User;
using PowerArgs;

namespace Edelstein.Common.Gameplay.Stages.Game.Commands.Admin
{
    public class SkillRecordCommandArgs : CommandArgs
    {
        [ArgPosition(0), ArgRequired]
        [ArgDescription("The skill")]
        public Skill Skill { get; set; }

        [ArgPosition(1), ArgRequired]
        [ArgDescription("The skill level")]
        public int Level { get; set; }

        [ArgPosition(2)]
        [ArgDescription("The skill master level")]
        public int MasterLevel { get; set; } = 10;
    }

    public class SkillRecordCommand : AbstractCommand<SkillRecordCommandArgs>
    {
        public override string Name => "SkillRecord";
        public override string Description => "Sets the character skill record to the desired values";

        public SkillRecordCommand()
        {
            Aliases.Add("Skill");
        }

        public override async Task Execute(IFieldObjUser user, SkillRecordCommandArgs args)
        {
            await user.ModifySkills(s => s.Set(
                (int)args.Skill,
                args.Level,
                GameConstants.IsSkillNeedMasterLevel((int)args.Skill) ? args.MasterLevel : null)
            );
        }
    }
}
