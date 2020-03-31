namespace Edelstein.Core.Gameplay.Social.Party.Events
{
    public class PartyChangeBossEvent : IPartyMemberEvent
    {
        public int PartyID { get; }
        public int PartyMemberID { get; }

        public bool Disconnect { get; }

        public PartyChangeBossEvent(int partyID, int partyMemberID, bool disconnect)
        {
            PartyID = partyID;
            PartyMemberID = partyMemberID;
            Disconnect = disconnect;
        }
    }
}