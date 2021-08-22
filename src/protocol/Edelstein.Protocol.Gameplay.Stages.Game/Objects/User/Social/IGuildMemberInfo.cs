namespace Edelstein.Protocol.Gameplay.Stages.Game.Objects.User.Social
{
    public interface IGuildMemberInfo
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
