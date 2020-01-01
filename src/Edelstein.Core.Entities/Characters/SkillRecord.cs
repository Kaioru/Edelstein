using System;

namespace Edelstein.Entities.Characters
{
    public class SkillRecord
    {
        public int Level { get; set; }
        public int MasterLevel { get; set; }
        public DateTime? DateExpire { get; set; }
    }
}