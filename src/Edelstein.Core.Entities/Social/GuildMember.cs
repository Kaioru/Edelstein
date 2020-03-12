namespace Edelstein.Entities.Social
{
    public class GuildMember
    {
        public int CharacterID { get; set; }
        public string CharacterName { get; set; }
        public int Job;
        public int Level;
        public int Grade;
        public bool Online;
        public int Commitment;
    }
}