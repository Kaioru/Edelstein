using System;

namespace Edelstein.Service.Game.Fields.User.Stats
{
    public class TemporaryStat
    {
        public TemporaryStatType Type { get; set; }

        public int Option { get; set; }
        public int TemplateID { get; set; }
        public DateTime? DateExpire { get; set; }
    }
}