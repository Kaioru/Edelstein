namespace Edelstein.Protocol.Gameplay.Stages.Game.Movements;

public interface IMoveAction
{
    MoveActionType Type { get; }
    MoveActionDirection Direction { get; }

    byte Raw { get; }
}
