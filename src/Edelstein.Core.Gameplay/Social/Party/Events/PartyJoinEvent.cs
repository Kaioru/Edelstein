using System.Collections.Generic;
using Edelstein.Entities.Social;

namespace Edelstein.Core.Gameplay.Social.Party.Events
{
    public class PartyJoinEvent : IPartyMemberEvent
    {
        public int PartyID { get; }
        public int PartyMemberID { get; }

        public Entities.Social.Party Party { get; }
        public ICollection<PartyMember> PartyMembers { get; }

        public PartyJoinEvent(
            int partyID,
            int partyMemberID,
            Entities.Social.Party party,
            ICollection<PartyMember> partyMembers
        )
        {
            PartyID = partyID;
            PartyMemberID = partyMemberID;
            Party = party;
            PartyMembers = partyMembers;
        }
    }
}