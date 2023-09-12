using Edelstein.Protocol.Data;
using Edelstein.Protocol.Gameplay.Models.Characters.Skills.Templates;

namespace Edelstein.Common.Gameplay.Models.Characters.Skills.Templates;

public class SkillStringTemplate : ISkillStringTemplate
{
    public int ID { get; }
    
    public string Name { get; }
    public string Desc { get; }
    
    public SkillStringTemplate(int id, IDataProperty property)
    {
        ID = id;

        Name = property.ResolveOrDefault<string>("name") ?? string.Empty;
        Desc = property.ResolveOrDefault<string>("desc") ?? string.Empty;
    }
}
