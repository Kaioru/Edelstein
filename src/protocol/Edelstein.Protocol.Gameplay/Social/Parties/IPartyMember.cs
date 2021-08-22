namespace Edelstein.Protocol.Gameplay.Social.Parties
{
    public interface IPartyMember
    {
        int CharacterID { get; }
        string CharacterName { get; }

        int Job { get; }
        int Level { get; }
        int ChannelID { get; }
        int FieldID { get; }
    }
}
