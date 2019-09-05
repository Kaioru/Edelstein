namespace Edelstein.Core.Gameplay.Social.Messages
{
    public class SocialUpdateJobMessage : ISocialUpdateMessage
    {
        public int CharacterID { get; set; }

        public short Job { get; set; }
    }
}