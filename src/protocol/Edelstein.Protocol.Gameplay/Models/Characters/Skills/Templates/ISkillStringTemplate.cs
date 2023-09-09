using Edelstein.Protocol.Utilities.Templates;

namespace Edelstein.Protocol.Gameplay.Models.Characters.Skills.Templates;

public interface ISkillStringTemplate : ITemplate
{
    string Name { get; }
    string Desc { get; }
}
