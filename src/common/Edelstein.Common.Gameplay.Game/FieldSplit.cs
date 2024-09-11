using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Edelstein.Common.Gameplay.Game.Objects;
using Edelstein.Protocol.Gameplay.Game;
using Edelstein.Protocol.Gameplay.Game.Objects;
using Edelstein.Protocol.Gameplay.Game.Objects.Users;

namespace Edelstein.Common.Gameplay.Game;

public class FieldSplit(
    int row, 
    int col
) : AbstractFieldObjectPool, IFieldSplit
{
    private readonly HashSet<IFieldObject> _objects = new();
    private readonly HashSet<IFieldUser> _observers = new();
    private readonly SemaphoreSlim _lock = new(1, 1);
    
    public int Row { get; } = row;
    public int Col { get; } = col;

    public override IFieldObject? GetObject(int id)
        => _objects.FirstOrDefault(o => o.ObjectID == id);
    
    public override IEnumerable<IFieldObject> GetObjects()
        => _objects;

    public IEnumerable<IFieldUser> GetObservers()
        => _observers;
    
    public override async Task Enter(IFieldObject obj)
    {
        await _lock.WaitAsync();

        try
        {
            var from = obj.FieldSplit;

            if (from != null)
                await from.MigrateOut(obj);
            await MigrateIn(obj);

            obj.FieldSplit = this;

            var toObservers = GetObservers()
                .ToImmutableList();
            var fromObservers = from?.GetObservers()
                .ToImmutableList() ?? ImmutableList<IFieldUser>.Empty;
            var newWatchers = toObservers
                .Where(w => w != obj)
                .Where(obj.IsVisibleTo)
                .Except(fromObservers)
                .ToImmutableArray();
            var oldWatchers = fromObservers
                .Where(w => w != obj)
                .Where(obj.IsVisibleTo)
                .Except(toObservers)
                .ToImmutableArray();

            var dispatchEnter = obj.GetDispatchEnterField(true);
            var dispatchLeave = obj.GetDispatchLeaveField();

            await Task.WhenAll(newWatchers.Select(w => w.Dispatch(dispatchEnter)));
            await Task.WhenAll(oldWatchers.Select(w => w.Dispatch(dispatchLeave)));

            if (obj is IFieldUser observer)
            {
                var enclosingSplits = observer.Field?.GetEnclosingSplits(this) ?? Array.Empty<IFieldSplit>();
                var oldSplits = observer.Observing
                    .Except(enclosingSplits)
                    .Where(s => s != null)
                    .ToImmutableArray();
                var newSplits = enclosingSplits
                    .Except(observer.Observing)
                    .Where(s => s != null)
                    .ToImmutableArray();

                await Task.WhenAll(oldSplits.Select(s => s!.Unobserve(observer)));
                await Task.WhenAll(newSplits.Select(s => s!.Observe(observer)));
            }
        }
        finally
        {
            _lock.Release();
        }
    }
    
    public override async Task Leave(IFieldObject obj)
    {
        await _lock.WaitAsync();
        
        try {
            obj.FieldSplit = null;

            await MigrateOut(obj);
            await Dispatch(obj.GetDispatchLeaveField(true), obj);
        }
        finally
        {
            _lock.Release();
        }
    }
    
    public Task MigrateIn(IFieldObject obj)
    {
        _objects.Add(obj);
        return Task.CompletedTask;
    }

    public Task MigrateOut(IFieldObject obj)
    {
        _objects.Remove(obj);
        return Task.CompletedTask;
    }
    
    public async Task Observe(IFieldUser user)
    {
        _observers.Add(user);
        user.Observing.Add(this);

        await Task.WhenAll(_objects
            .Where(o => o != user)
            .Where(o => o.IsVisibleTo(user))
            .Select(o => user.Dispatch(o.GetDispatchEnterField())));
    }
    
    public async Task Unobserve(IFieldUser user, bool isLeaveField = false)
    {
        _observers.Remove(user);
        user.Observing.Remove(this);

        if (!isLeaveField)
            await Task.WhenAll(_objects
                .Where(o => o != user)
                .Where(o => o.IsVisibleTo(user))
                .Select(o => user.Dispatch(o.GetDispatchLeaveField())));
    }
}
