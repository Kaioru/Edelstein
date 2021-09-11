using System;

namespace Edelstein.Protocol.Gameplay.Users.Skills
{
    public record CharacterSkillRecord
    {
        public int Level { get; set; }
        public int? MasterLevel { get; set; }
        public DateTime? DateExpire { get; set; }
    }
}
