namespace Edelstein.Protocol.Gameplay.Stages.Game.Objects;

public interface IFieldObjectPool
{
    IReadOnlyCollection<IFieldObject> Objects { get; }

    IFieldObject? GetObject(int id);
    T? GetObject<T>(int id) where T : IFieldObject;

    IEnumerable<IFieldObject> GetObjects();
    IEnumerable<T> GetObjects<T>() where T : IFieldObject;
}
