namespace Edelstein.Protocol.Gameplay.Models.Characters.Skills;

public interface ISkillRecord
{
    int Level { get; set; }
    int? MasterLevel { get; set; }
    DateTime? DateExpire { get; set; }
}
