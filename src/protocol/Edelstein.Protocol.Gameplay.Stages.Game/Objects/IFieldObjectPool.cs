namespace Edelstein.Protocol.Gameplay.Stages.Game.Objects;

public interface IFieldObjectPool
{
    IReadOnlyCollection<IFieldObject?> Objects { get; }

    Task Enter(IFieldObject obj);
    Task Leave(IFieldObject obj);

    IFieldObject? GetObject(int id);
    IEnumerable<IFieldObject?> GetObjects();
}
