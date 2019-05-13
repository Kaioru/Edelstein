using System.Threading.Tasks;
using CommandLine;
using Edelstein.Service.Game.Fields.User;

namespace Edelstein.Service.Game.Commands.Handling
{
    public class SkillCommand : AbstractCommand<SkillCommandOption>
    {
        public override string Name => "Skill";
        public override string Description => "Sets the specified skill to specific values";

        public SkillCommand(Parser parser) : base(parser)
        {
        }

        protected override async Task Execute(FieldUser sender, SkillCommandOption option)
        {
            await sender.ModifySkills(s =>
                s.Set(
                    option.Skill,
                    option.Level ?? sender.Character.GetSkillLevel(option.Skill),
                    option.MasterLevel ?? sender.Character.GetSkillMasterLevel(option.Skill)
                ));
        }
    }

    public class SkillCommandOption
    {
        [Value(0, MetaName = "skill", HelpText = "The skill ID.", Required = true)]
        public int Skill { get; set; }

        [Option('l', "level", HelpText = "The skill level.")]
        public byte? Level { get; set; }

        [Option('m', "masterLevel", HelpText = "The skill master level.")]
        public byte? MasterLevel { get; set; }
    }
}