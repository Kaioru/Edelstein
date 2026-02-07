using Edelstein.Protocol.Gameplay.Game.Objects.User;

namespace Edelstein.Protocol.Gameplay.Game.Rates;

public interface IRateContext
{
    IFieldUser? User { get; }
    IGameStageOptions? Options { get; }
}
