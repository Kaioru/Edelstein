using System;

namespace Edelstein.Service.Game.Fields.Objects.Mob.Stats
{
    public class MobStat
    {
        public MobStat Type { get; set; }

        public int Option { get; set; }
        public int TemplateID { get; set; }
        public DateTime? DateExpire { get; set; }
    }
}