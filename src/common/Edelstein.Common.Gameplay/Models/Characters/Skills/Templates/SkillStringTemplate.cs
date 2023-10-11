using Duey.Abstractions;
using Edelstein.Protocol.Gameplay.Models.Characters.Skills.Templates;

namespace Edelstein.Common.Gameplay.Models.Characters.Skills.Templates;

public class SkillStringTemplate : ISkillStringTemplate
{
    public int ID { get; }
    
    public string Name { get; }
    public string Desc { get; }
    
    public SkillStringTemplate(int id, IDataNode node)
    {
        ID = id;

        Name = node.ResolveString("name") ?? string.Empty;
        Desc = node.ResolveString("desc") ?? string.Empty;
    }
}
