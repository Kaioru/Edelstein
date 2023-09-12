using Edelstein.Protocol.Gameplay.Models.Characters.Skills;

namespace Edelstein.Common.Gameplay.Models.Characters.Skills;

public record SkillRecord : ISkillRecord
{
    public int Level { get; set; }
    public int? MasterLevel { get; set; }
    public DateTime? DateExpire { get; set; }
}
