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
        public ICollection<GuildMember> Members { get; set; }
        
        public short MarkBg { get; set; }
        public byte MarkBgColor { get; set; }
        public short Mark { get; set; }
        public byte MarkColor { get; set; }

        public string Notice;
        public int Point;
        public int Level;

        public Guild()
        {
            GradeName = new string[6];
        }
    }
}