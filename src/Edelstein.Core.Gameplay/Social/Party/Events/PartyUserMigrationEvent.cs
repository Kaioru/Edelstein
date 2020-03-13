namespace Edelstein.Core.Gameplay.Social.Party.Events
{
    public class PartyUserMigrationEvent : IPartyMemberEvent
    {
        public int PartyID { get; }
        public int PartyMemberID { get; }
        public int ChannelID { get; }
        public int FieldID { get; }

        public PartyUserMigrationEvent(int partyID, int partyMemberID, int channelID, int fieldID)
        {
            PartyID = partyID;
            PartyMemberID = partyMemberID;
            ChannelID = channelID;
            FieldID = fieldID;
        }
    }
}