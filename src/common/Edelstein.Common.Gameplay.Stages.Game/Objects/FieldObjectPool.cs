using System.Collections.Concurrent;
using System.Collections.Immutable;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects;

namespace Edelstein.Common.Gameplay.Stages.Game.Objects;

public class FieldObjectPool : IFieldObjectPool
{
    private readonly IDictionary<int, IFieldObject?> _objects;
    private readonly Queue<int> _runningObjectID;

    public FieldObjectPool()
    {
        _objects = new ConcurrentDictionary<int, IFieldObject?>();
        _runningObjectID = new Queue<int>(Enumerable.Range(1, 30_000));
    }

    public IReadOnlyCollection<IFieldObject?> Objects => _objects.Values.ToImmutableList();

    public Task Enter(IFieldObject obj)
    {
        obj.ObjectID = _runningObjectID.Dequeue();
        _objects[obj.ObjectID.Value] = obj;
        return Task.CompletedTask;
    }

    public Task Leave(IFieldObject obj)
    {
        var objectID = obj.ObjectID;

        if (objectID == null) return Task.CompletedTask;

        _objects.Remove(objectID.Value);
        _runningObjectID.Enqueue(objectID.Value);

        obj.ObjectID = null;

        return Task.CompletedTask;
    }

    public IFieldObject? GetObject(int id) => _objects.TryGetValue(id, out var obj)
        ? obj
        : null;

    public IEnumerable<IFieldObject?> GetObjects() => Objects;
}
