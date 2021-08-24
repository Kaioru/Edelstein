using System;
using System.Collections.Generic;
using Edelstein.Protocol.Datastore;

namespace Edelstein.Protocol.Gameplay.Social.Parties
{
    public class PartyRecord : IParty, IDataDocument
    {
        public int ID { get; init; }
        public int BossCharacterID { get; set; }

        public ICollection<IPartyMember> Members { get; }

        public DateTime DateDocumentCreated { get; set; }
        public DateTime DateDocumentUpdated { get; set; }

        public PartyRecord()
        {
            Members = new List<IPartyMember>();
        }
    }
}
