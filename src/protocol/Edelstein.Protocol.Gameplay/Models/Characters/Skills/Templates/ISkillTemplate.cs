using Edelstein.Protocol.Utilities.Templates;

namespace Edelstein.Protocol.Gameplay.Models.Characters.Skills.Templates;

public interface ISkillTemplate : ITemplate
{
    ISkillTemplateLevel? this[int level] { get; }

    short MaxLevel { get; }

    bool IsPSD { get; }
    bool IsSummon { get; }
    bool IsInvisible { get; }
    bool IsCombatOrders { get; }
    
    Element Element { get; }
    
    int Delay { get; }

    ICollection<int> PsdSkill { get; }
    IDictionary<int, int> ReqSkill { get; } 
    IDictionary<int, ISkillTemplateLevel> Levels { get; }
}
