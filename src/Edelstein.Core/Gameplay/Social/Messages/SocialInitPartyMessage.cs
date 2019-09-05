namespace Edelstein.Core.Gameplay.Social.Messages
{
    public class SocialInitPartyMessage : ISocialMessage
    {
        public int CharacterID { get; set; }
        public PartyData Data { get; set; }
    }
}