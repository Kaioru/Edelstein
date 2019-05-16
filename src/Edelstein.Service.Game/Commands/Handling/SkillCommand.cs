using System.Threading.Tasks;
using CommandLine;
using Edelstein.Provider.Templates.Skill;
using Edelstein.Provider.Templates.String;
using Edelstein.Service.Game.Fields.User;

namespace Edelstein.Service.Game.Commands.Handling
{
    public class SkillCommand : AbstractTemplateCommand<SkillTemplate, SkillStringTemplate, SkillCommandOption>
    {
        public override string Name => "Skill";
        public override string Description => "Sets the specified skill to specific values";

        public SkillCommand(Parser parser) : base(parser)
        {
        }

        protected override async Task ExecuteAfter(FieldUser sender, SkillTemplate template, SkillCommandOption option)
        {
            await sender.ModifySkills(s =>
                s.Set(
                    template.ID,
                    option.Level ?? sender.Character.GetSkillLevel(template.ID),
                    option.MasterLevel ?? sender.Character.GetSkillMasterLevel(template.ID)
                ));
        }
    }

    public class SkillCommandOption : TemplateCommandOption
    {
        [Option('l', "level", HelpText = "The skill level.")]
        public byte? Level { get; set; }

        [Option('m', "masterLevel", HelpText = "The skill master level.")]
        public byte? MasterLevel { get; set; }
    }
}