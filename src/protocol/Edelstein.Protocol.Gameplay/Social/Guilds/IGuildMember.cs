namespace Edelstein.Protocol.Gameplay.Social.Guilds
{
    public interface IGuildMember
    {
        int CharacterID { get; }
        string CharacterName { get; }

        int Job { get; }
        int Level { get; }
        int Grade { get; }
        bool Online { get; }

        int Commitment { get; }
        bool Inactive { get; }
    }
}
