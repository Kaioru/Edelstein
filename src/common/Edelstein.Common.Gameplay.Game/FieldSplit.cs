using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Edelstein.Common.Gameplay.Game.Objects;
using Edelstein.Protocol.Gameplay.Game;
using Edelstein.Protocol.Gameplay.Game.Objects;
using Edelstein.Protocol.Network.Packets;

namespace Edelstein.Common.Gameplay.Game;

public class FieldSplit(
    int row, 
    int col
) : AbstractFieldObjectPool, IFieldSplit
{
    private readonly HashSet<IFieldObject> _objects = new();
    private readonly HashSet<IFieldSplitObserver> _observers = new();
    private readonly SemaphoreSlim _lock = new(1, 1);
    
    public int Row { get; } = row;
    public int Col { get; } = col;

    public override IFieldObject? GetObject(int id)
        => _objects.FirstOrDefault(o => o.ObjectID == id);
    
    public override IEnumerable<IFieldObject> GetObjects()
        => _objects;

    public IEnumerable<IFieldSplitObserver> GetObservers()
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
                .ToImmutableList() ?? ImmutableList<IFieldSplitObserver>.Empty;
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

            if (obj is IFieldSplitObserver observer)
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
            
            await UpdateControllableObjects();
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
            await UpdateControllableObjects();
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
    
    public async Task Observe(IFieldSplitObserver observer)
    {
        _observers.Add(observer);
        observer.Observing.Add(this);

        await Task.WhenAll(_objects
            .Where(o => o != observer)
            .Where(o => o.IsVisibleTo(observer))
            .Select(o => observer.Dispatch(o.GetDispatchEnterField())));
        await UpdateControllableObjects();
    }
    
    public async Task Unobserve(IFieldSplitObserver observer, bool isLeaveField = false)
    {
        _observers.Remove(observer);
        observer.Observing.Remove(this);

        if (!isLeaveField)
            await Task.WhenAll(_objects
                .Where(o => o != observer)
                .Where(o => o.IsVisibleTo(observer))
                .Select(o => observer.Dispatch(o.GetDispatchLeaveField())));
        await UpdateControllableObjects();
    }
    
    public override Task Dispatch(IDispatchable dispatch, IFieldObject? source = null)
        => Task.WhenAll(_observers
            .Where(o => o != source)
            .Select(o => o.Dispatch(dispatch)));
    
    private async Task UpdateControllableObjects()
    {
        var controllers = GetObservers()
            .OfType<IFieldObjectController>()
            .OrderBy(u => u.Controlling.Count)
            .ToImmutableArray();
        var controlled = GetObjects()
            .OfType<IFieldObjectControllable>()
            .ToImmutableArray();

        await Task.WhenAll(controlled
            .Where(c => c.Controller == null || !controllers.Contains(c.Controller))
            .Select(c => c.Control(controllers.FirstOrDefault(u => u.IsVisibleTo(c)))));
    }
}
