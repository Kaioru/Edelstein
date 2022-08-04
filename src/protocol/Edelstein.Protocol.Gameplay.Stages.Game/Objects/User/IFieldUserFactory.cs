namespace Edelstein.Protocol.Gameplay.Stages.Game.Objects.User;

public interface IFieldUserFactory
{
    IFieldUser? CreateUser(IGameStageUser user);
}
