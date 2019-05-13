using System;

namespace Edelstein.Database.Entities.Characters.Records
{
    public class SkillRecord
    {
        public int Level { get; set; }
        public int MasterLevel { get; set; }
        public DateTime? DateExpire { get; set; }
    }
}