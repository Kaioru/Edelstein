using Edelstein.Common.Constants;
using Edelstein.Protocol.Gameplay.Game.Objects.User;
using Edelstein.Protocol.Gameplay.Models.Characters.Skills.Templates;
using Edelstein.Protocol.Utilities.Templates;

namespace Edelstein.Plugin.Rue.Commands.Admin;

public sealed class SkillCommand : AbstractTemplateCommand<ISkillTemplate>
{
    public override string Name => "Skill";
    public override string Description => "Set skill level";

    private readonly ITemplateManager<ISkillStringTemplate> _strings;

    public SkillCommand(
        ITemplateManager<ISkillTemplate> templates,
        ITemplateManager<ISkillStringTemplate> strings
    ) : base(templates)
    {
        _strings = strings;

        Aliases.Add("SkillRecord");
        Insert(new SkillResetAllCommand()).Wait();
    }

    protected override async Task<IReadOnlyList<TemplateCommandIndex>> Indices()
    {
        var strings = await _strings.RetrieveAll();
        var result = new TemplateCommandIndex[strings.Count * 3];
        var i = 0;

        foreach (var s in strings)
        {
            result[i++] = TemplateCommandIndex.CreateFromId(s.ID, s.Name);
            result[i++] = TemplateCommandIndex.Create(s.ID, s.Name, s.Name);
            result[i++] = TemplateCommandIndex.CreateDescription(s.ID, s.Desc ?? string.Empty, s.Name);
        }

        return result;
    }

    protected override async Task Execute(IFieldUser user, ISkillTemplate template, TemplateCommandArgs args)
    {
        var skillLevel = await user.Prompt(target => target.AskNumber(
            "What skill level would you like to set?",
            user.Character.Skills[template.ID]?.Level ?? 0,
            0,
            template.MaxLevel
        ), -1);
        if (skillLevel == -1) return;

        await user.ModifySkills(s => s.Set(template, skillLevel, SkillConstants.IsSkillNeedMasterLevel(template.ID) ? skillLevel : null));
        await user.Message($"Successfully set skill {template.ID} level to {skillLevel}");
    }
}

public class SkillResetAllCommand : AbstractCommand
{
    public override string Name => "ResetAll";
    public override string Description => "Resets all skill records to 0";

    public override Task Execute(IFieldUser user, string[] args)
        => user.ModifySkills(s => s.ResetAll());
}
