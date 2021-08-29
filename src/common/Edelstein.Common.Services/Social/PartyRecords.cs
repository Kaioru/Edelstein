using System;
using System.Collections.Generic;
using System.Linq;
using Edelstein.Protocol.Datastore;
using Edelstein.Protocol.Gameplay.Social;
using Edelstein.Protocol.Services.Contracts.Social;

namespace Edelstein.Common.Services.Social
{
    public record PartyRecord : IDataDocument, IParty
    {
        public int ID { get; set; }

        public int Boss { get; set; }
        public ICollection<PartyMemberRecord> Members { get; set; }

        ICollection<IPartyMember> IParty.Members => Members.ToList<IPartyMember>();

        public DateTime DateDocumentCreated { get; set; }
        public DateTime DateDocumentUpdated { get; set; }

        public PartyRecord() => Members = new List<PartyMemberRecord>();
        public PartyRecord(PartyContract contract) => FromContract(contract);

        public void FromContract(PartyContract contract)
        {
            ID = contract.Id;
            Boss = contract.Boss;
            Members = contract.Members.Select(m => new PartyMemberRecord(m)).ToList();
        }
    }

    public record PartyMemberRecord : IPartyMember
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int Job { get; set; }
        public int Level { get; set; }
        public int Channel { get; set; }
        public int Field { get; set; }

        public PartyMemberRecord() { }
        public PartyMemberRecord(PartyMemberContract contract) => FromContract(contract);

        public void FromContract(PartyMemberContract contract)
        {
            ID = contract.Id;
            Name = contract.Name;
            Job = contract.Job;
            Level = contract.Level;
            Channel = contract.Channel;
            Field = contract.Field;
        }
    }

    internal static class PartyContracts
    {
        public static PartyContract ToContract(this IParty party)
        {
            var contract = new PartyContract { Id = party.ID, Boss = party.Boss };

            contract.Members.Add(party.Members.Select(m => m.ToContract()).ToList());

            return contract;
        }

        public static PartyMemberContract ToContract(this IPartyMember member)
            => new()
            {
                Id = member.ID,
                Name = member.Name ?? string.Empty,
                Job = member.Job,
                Level = member.Level,
                Channel = member.Channel,
                Field = member.Field
            };
    }
}
