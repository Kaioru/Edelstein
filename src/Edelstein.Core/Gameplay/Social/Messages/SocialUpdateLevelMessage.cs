namespace Edelstein.Core.Gameplay.Social.Messages
{
    public class SocialUpdateLevelMessage : ISocialUpdateMessage
    {
        public int CharacterID { get; set; }

        public byte Level { get; set; }
    }
}