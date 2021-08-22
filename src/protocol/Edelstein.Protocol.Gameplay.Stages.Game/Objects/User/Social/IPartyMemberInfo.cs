namespace Edelstein.Protocol.Gameplay.Stages.Game.Objects.User.Social
{
    public interface IPartyMemberInfo
    {
        int CharacterID { get; }
        string CharacterName { get; }

        int Job { get; }
        int Level { get; }
        int ChannelID { get; }
        int FieldID { get; }
    }
}
