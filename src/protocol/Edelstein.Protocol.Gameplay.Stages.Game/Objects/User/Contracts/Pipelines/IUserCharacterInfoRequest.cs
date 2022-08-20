namespace Edelstein.Protocol.Gameplay.Stages.Game.Objects.User.Contracts.Pipelines;

public interface IUserCharacterInfoRequest : IFieldUserContract
{
    int CharacterID { get; }
}
