namespace Edelstein.Protocol.Gameplay.Social.Guilds
{
    public class GuildMemberRecord : IGuildMember
    {
        public int CharacterID { get; init; }
        public string CharacterName { get; set; }

        public int Job { get; set; }
        public int Level { get; set; }
        public int Grade { get; set; }
        public bool Online { get; set; }

        public int Commitment { get; set; }
        public bool Inactive { get; set; }
    }
}
