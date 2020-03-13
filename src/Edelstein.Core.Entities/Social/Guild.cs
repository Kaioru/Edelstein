using System.Collections.Generic;
using Edelstein.Database;

namespace Edelstein.Entities.Social
{
    public class Guild : IDataEntity
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public string[] GradeName { get; }

        public int MaxMemberNum { get; set; }

        public short MarkBg { get; set; }
        public byte MarkBgColor { get; set; }
        public short Mark { get; set; }
        public byte MarkColor { get; set; }

        public string Notice { get; set; }
        public int Point { get; set; }
        public byte Level { get; set; }

        public Guild()
        {
            GradeName = new[]
            {
                "Master",
                "Jr. Master",
                "Member",
                "Member",
                "Member"
            };
        }
    }
}