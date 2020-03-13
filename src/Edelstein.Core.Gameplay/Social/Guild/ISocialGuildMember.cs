namespace Edelstein.Core.Gameplay.Social.Guild
{
    public interface ISocialGuildMember
    {
        public int CharacterID { get; }
        public string CharacterName { get; }
        public int Job { get; }
        public int Level { get; }
        public int Grade { get; }
        public bool Online { get; }
        public int Commitment { get; }
    }
}