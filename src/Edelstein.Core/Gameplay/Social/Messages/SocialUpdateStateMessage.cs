using Edelstein.Core.Distributed.Migrations;

namespace Edelstein.Core.Gameplay.Social.Messages
{
    public class SocialUpdateStateMessage : ISocialUpdateMessage
    {
        public int CharacterID { get; set; }

        public MigrationState State { get; set; }
        public string Service { get; set; }
    }
}