namespace Edelstein.Protocol.Gameplay.Game.Movements;

public interface IMoveAction
{
    MoveActionType Type { get; }
    MoveActionDirection Direction { get; }

    byte Raw { get; }
}
