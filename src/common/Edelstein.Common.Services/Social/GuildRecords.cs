using System;
using System.Collections.Generic;
using System.Linq;
using Edelstein.Protocol.Datastore;
using Edelstein.Protocol.Gameplay.Social;
using Edelstein.Protocol.Services.Contracts.Social;

namespace Edelstein.Common.Services.Social
{
    public record GuildRecord : IDataDocument, IGuild
    {
        public int ID { get; set; }

        public string Name { get; set; }
        public string[] Grade { get; set; }

        public int MaxMemberNum { get; set; }
        public ICollection<GuildMemberRecord> Members { get; set; }

        ICollection<IGuildMember> IGuild.Members => Members.ToList<IGuildMember>();

        public short MarkBg { get; set; }
        public byte MarkBgColor { get; set; }
        public short Mark { get; set; }
        public byte MarkColor { get; set; }

        public string Notice { get; set; }
        public int Point { get; set; }
        public byte Level { get; set; }

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
            Members = contract.Members
                .Select(m => new GuildMemberRecord(m))
                .ToList();
            MarkBg = (short)contract.MarkBg;
            MarkBgColor = (byte)contract.MarkBgColor;
            Mark = (short)contract.Mark;
            MarkColor = (byte)contract.MarkColor;
            Notice = contract.Notice;
            Point = contract.Point;
            Level = (byte)contract.Level;
        }
    }

    public record GuildMemberRecord : IGuildMember
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
    }

    internal static class GuildContracts
    {
        public static GuildContract ToContract(this IGuild guild)
        {
            var contract = new GuildContract
            {
                Id = guild.ID,
                Name = guild.Name ?? string.Empty,
                MaxMemberNum = guild.MaxMemberNum,
                MarkBg = guild.MarkBg,
                MarkBgColor = guild.MarkBgColor,
                Mark = guild.Mark,
                MarkColor = guild.MarkColor,
                Notice = guild.Notice ?? string.Empty,
                Point = guild.Point,
                Level = guild.Level
            };

            contract.Grade.Add(guild.Grade);
            contract.Members.Add(guild.Members.Select(m => m.ToContract()).ToList());

            return contract;
        }

        public static GuildMemberContract ToContract(this IGuildMember member)
            => new()
            {
                Id = member.ID,
                Name = member.Name ?? string.Empty,
                Job = member.Job,
                Level = member.Level,
                Grade = member.Grade,
                Online = member.Online,
                Commitment = member.Commitment
            };
    }
}