namespace Edelstein.Core.Gameplay.Social.Party.Events
{
    public class PartyChangeLevelOrJobEvent : IPartyMemberEvent
    {
        public int PartyID { get; }
        public int PartyMemberID { get; }
        public int Level { get; }
        public int Job { get; }

        public PartyChangeLevelOrJobEvent(
            int partyID,
            int partyMemberID,
            int level,
            int job
        )
        {
            PartyID = partyID;
            PartyMemberID = partyMemberID;
            Level = level;
            Job = job;
        }
    }
}