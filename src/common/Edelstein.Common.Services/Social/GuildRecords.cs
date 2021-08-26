using System;
using System.Collections.Generic;
using System.Linq;
using Edelstein.Protocol.Datastore;
using Edelstein.Protocol.Services.Contracts.Social;

namespace Edelstein.Common.Services.Social
{
    public class GuildRecord : IDataDocument
    {
        public int ID { get; set; }

        public string Name { get; set; }
        public string[] Grade { get; set; }

        public int MaxMemberNum { get; set; }
        public ICollection<GuildMemberRecord> Members { get; set; }

        public int MarkBg { get; set; }
        public int MarkBgColor { get; set; }
        public int Mark { get; set; }
        public int MarkColor { get; set; }

        public string Notice { get; set; }
        public int Point { get; set; }
        public int Level { get; set; }

        public DateTime DateDocumentCreated { get; set; }
        public DateTime DateDocumentUpdated { get; set; }

        public GuildRecord()
        {
            Grade = Array.Empty<string>();
            Members = new List<GuildMemberRecord>();
        }
        public GuildRecord(GuildContract contract) => FromContract(contract);

        public void FromContract(GuildContract contract)
        {
            ID = contract.Id;
            Name = contract.Name;
            Grade = contract.Grade.ToArray();
            MaxMemberNum = contract.MaxMemberNum;
            Members = contract.Members.Select(m => new GuildMemberRecord(m)).ToList();
            MarkBg = contract.MarkBg;
            MarkBgColor = contract.MarkBgColor;
            Mark = contract.Mark;
            MarkColor = contract.MarkColor;
            Notice = contract.Notice;
            Point = contract.Point;
            Level = contract.Level;
        }

        public GuildContract ToContract()
        {
            var contract = new GuildContract
            {
                Id = ID,
                Name = Name ?? string.Empty,
                MaxMemberNum = MaxMemberNum,
                MarkBg = MarkBg,
                MarkBgColor = MarkBgColor,
                Mark = Mark,
                MarkColor = MarkColor,
                Notice = Notice ?? string.Empty,
                Point = Point,
                Level = Level
            };

            contract.Grade.Add(Grade);
            contract.Members.Add(Members.Select(m => m.ToContract()).ToList());

            return contract;
        }
    }

    public class GuildMemberRecord
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int Job { get; set; }
        public int Level { get; set; }
        public int Grade { get; set; }
        public bool Online { get; set; }
        public int Commitment { get; set; }

        public GuildMemberRecord() { }
        public GuildMemberRecord(GuildMemberContract contract) => FromContract(contract);

        public void FromContract(GuildMemberContract contract)
        {
            ID = contract.Id;
            Name = contract.Name;
            Job = contract.Job;
            Level = contract.Level;
            Grade = contract.Grade;
            Online = contract.Online;
            Commitment = contract.Commitment;
        }

        public GuildMemberContract ToContract()
            => new()
            {
                Id = ID,
                Name = Name ?? string.Empty,
                Job = Job,
                Level = Level,
                Grade = Grade,
                Online = Online,
                Commitment = Commitment
            };
    }
}
