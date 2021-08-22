using System;
using System.Collections.Generic;
using Edelstein.Protocol.Datastore;

namespace Edelstein.Protocol.Gameplay.Social.Parties
{
    public class Party : IParty, IDataDocument
    {
        public int ID { get; init; }
        public int BossCharacterID { get; set; }

        public ICollection<IPartyMember> Members { get; }

        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }

        public Party()
        {
            Members = new List<IPartyMember>();
        }
    }
}
