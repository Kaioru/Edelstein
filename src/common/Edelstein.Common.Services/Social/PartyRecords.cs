using System;
using System.Collections.Generic;
using System.Linq;
using Edelstein.Protocol.Datastore;
using Edelstein.Protocol.Services.Contracts.Social;

namespace Edelstein.Common.Services.Social
{
    public record PartyRecord : IDataDocument
    {
        public int ID { get; set; }

        public int Boss { get; set; }
        public ICollection<PartyMemberRecord> Members { get; set; }

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

        public PartyContract ToContract()
        {
            var contract = new PartyContract { Id = ID, Boss = Boss };

            contract.Members.Add(Members.Select(m => m.ToContract()).ToList());

            return contract;
        }
    }

    public record PartyMemberRecord
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

        public PartyMemberContract ToContract()
            => new()
            {
                Id = ID,
                Name = Name,
                Job = Job,
                Level = Level,
                Channel = Channel,
                Field = Field
            };
    }
}
