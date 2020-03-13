namespace Edelstein.Entities.Social
{
    public class GuildMember
    {
        public int CharacterID { get; set; }
        public string CharacterName { get; set; }
        public int Job { get; set; }
        public int Level { get; set; }
        public int Grade { get; set; }
        public bool Online { get; set; }
        public int Commitment { get; set; }
    }
}