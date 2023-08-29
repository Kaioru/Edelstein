using Edelstein.Protocol.Utilities.Templates;

namespace Edelstein.Protocol.Gameplay.Models.Characters.Skills.Templates;

public interface ISkillTemplate : ITemplate
{
    short MaxLevel { get; }

    bool IsPSD { get; }
    bool IsSummon { get; }
    bool IsInvisible { get; }

    IDictionary<int, int> ReqSkill { get; } 
    IDictionary<int, ISkillTemplateLevel> Levels { get; }
}
