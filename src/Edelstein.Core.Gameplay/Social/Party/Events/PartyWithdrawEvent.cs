namespace Edelstein.Core.Gameplay.Social.Party.Events
{
    public class PartyWithdrawEvent : IPartyMemberEvent
    {
        public int PartyID { get; }
        public int PartyMemberID { get; }
        
        public bool Disband { get; }
        public int CharacterID { get; }
        public string CharacterName { get; }

        public PartyWithdrawEvent(int partyID, int partyMemberID, bool disband, int characterID, string characterName)
        {
            PartyID = partyID;
            PartyMemberID = partyMemberID;
            Disband = disband;
            CharacterID = characterID;
            CharacterName = characterName;
        }
    }
}