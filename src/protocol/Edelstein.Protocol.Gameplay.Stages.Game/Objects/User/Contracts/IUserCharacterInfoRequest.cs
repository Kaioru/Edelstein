namespace Edelstein.Protocol.Gameplay.Stages.Game.Objects.User.Contracts;

public interface IUserCharacterInfoRequest : IFieldUserContract
{
    int CharacterID { get; }
}
