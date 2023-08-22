using System.Collections.Concurrent;
using System.Collections.Immutable;
using Edelstein.Protocol.Gameplay.Game.Objects;
using Edelstein.Protocol.Gameplay.Game.Objects.User;

namespace Edelstein.Common.Gameplay.Game.Objects;

public class FieldObjectPool : AbstractFieldObjectPool, IFieldObjectPool
{
    private readonly IDictionary<int, IFieldObject> _objects;
    private readonly Queue<int> _runningObjectID;
    
    public override IReadOnlyCollection<IFieldObject> Objects => _objects.Values.ToImmutableList();
    
    public FieldObjectPool()
    {
        _objects = new ConcurrentDictionary<int, IFieldObject>();
        _runningObjectID = new Queue<int>(Enumerable.Range(1, 30_000));
    }

    public override Task Enter(IFieldObject obj)
    {
        if (obj is IFieldUser user) user.ObjectID = user.Character.ID;
        else obj.ObjectID = _runningObjectID.Dequeue();

        _objects[obj.ObjectID!.Value] = obj;
        return Task.CompletedTask;
    }

    public override Task Leave(IFieldObject obj)
    {
        var objectID = obj.ObjectID;

        if (objectID == null) return Task.CompletedTask;

        _objects.Remove(objectID.Value);

        if (obj is not IFieldUser)
            _runningObjectID.Enqueue(objectID.Value);

        obj.ObjectID = null;

        return Task.CompletedTask;
    }

    public override IFieldObject? GetObject(int id) => _objects.TryGetValue(id, out var obj)
        ? obj
        : null;
}
