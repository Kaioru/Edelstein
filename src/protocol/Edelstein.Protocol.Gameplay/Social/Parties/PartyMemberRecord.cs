namespace Edelstein.Protocol.Gameplay.Social.Parties
{
    public class PartyMemberRecord : IPartyMember
    {
        public int CharacterID { get; init; }
        public string CharacterName { get; set; }

        public int Job { get; set; }
        public int Level { get; set; }
        public int ChannelID { get; set; }
        public int FieldID { get; set; }
    }
}
