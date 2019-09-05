namespace Edelstein.Core.Gameplay.Social.Messages
{
    public class SocialLevelMessage : ISocialMessage
    {
        public int CharacterID { get; set; }

        public byte Level { get; set; }
    }
}