namespace Edelstein.Protocol.Gameplay.Stages.Game.Objects.User.Contracts.Pipelines;

public interface IUserEmotion : IFieldUserContract
{
    int Emotion { get; }
    int Duration { get; }
    bool ByItemOption { get; }
}
