namespace Edelstein.Core.Gameplay.Social.Party.Events
{
    public class PartyWithdrawEvent : IPartyMemberEvent
    {
        public int PartyID { get; }
        public int PartyMemberID { get; }
        
        public bool Disband { get; }
        public bool Kick { get; }
        public string CharacterName { get; }

        public PartyWithdrawEvent(int partyID, int partyMemberID, bool disband, bool kick, string characterName)
        {
            PartyID = partyID;
            PartyMemberID = partyMemberID;
            Disband = disband;
            Kick = kick;
            CharacterName = characterName;
        }
    }
}