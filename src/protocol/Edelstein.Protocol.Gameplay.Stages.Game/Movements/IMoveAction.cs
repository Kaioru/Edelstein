namespace Edelstein.Protocol.Gameplay.Stages.Game.Movements;

public interface IMoveAction
{
    MoveActionType Type { get; }
    bool IsFacingLeft { get; }

    byte Raw { get; }
}
