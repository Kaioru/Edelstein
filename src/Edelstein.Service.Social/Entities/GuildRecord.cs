using System.Collections.Generic;
using Edelstein.Database.Store;

namespace Edelstein.Service.Social.Entities
{
    public class GuildRecord : IDataEntity
    {
        public int ID { get; set; }

        public string Name { get; set; }
        public List<string> GradeName { get; set; } // TODO: array?
        public int MaxMemberNum { get; set; }

        public byte MarkBg { get; set; }
        public byte Mark { get; set; }
        public byte MarkColor { get; set; }

        public string Notice { get; set; }

        public int Point { get; set; }
        public int Level { get; set; }

        // TODO: alliance
        // TODO: skill record
    }
}