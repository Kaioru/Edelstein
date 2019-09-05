using Edelstein.Core.Distributed.Migrations;

namespace Edelstein.Core.Gameplay.Social.Messages
{
    public class SocialStateMessage : ISocialMessage
    {
        public int CharacterID { get; set; }

        public MigrationState State { get; set; }
        public string Service { get; set; }
    }
}