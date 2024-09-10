using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Edelstein.Protocol.Gameplay.Game.Objects;
using Edelstein.Protocol.Gameplay.Game.Objects.Users;

namespace Edelstein.Common.Gameplay.Game.Objects;

public class FieldObjectPool : AbstractFieldObjectPool, IFieldObjectPool
{
    private readonly ConcurrentDictionary<int, IFieldObject> _objects = new();
    private readonly Queue<int> _runningObjectID = new(Enumerable.Range(1, 99_999));
    
    public override IFieldObject? GetObject(int id) 
        =>  _objects.TryGetValue(id, out var obj) ? obj : null;
    
    public override IEnumerable<IFieldObject> GetObjects()
        => _objects.Values;

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

        if (objectID == null) 
            return Task.CompletedTask;

        _objects.Remove(objectID.Value, out _);
        
        if (obj is not IFieldUser)
            _runningObjectID.Enqueue(objectID.Value);

        obj.ObjectID = null;
        return Task.CompletedTask;
    }
}
