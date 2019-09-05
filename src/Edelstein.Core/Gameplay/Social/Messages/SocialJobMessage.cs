namespace Edelstein.Core.Gameplay.Social.Messages
{
    public class SocialJobMessage : ISocialMessage
    {
        public int CharacterID { get; set; }

        public short Job { get; set; }
    }
}