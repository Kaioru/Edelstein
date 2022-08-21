using System.Collections.Immutable;
using Edelstein.Common.Gameplay.Stages.Game.Contracts.Events;
using Edelstein.Common.Gameplay.Stages.Game.Objects;
using Edelstein.Protocol.Gameplay.Stages.Game;
using Edelstein.Protocol.Gameplay.Stages.Game.Contexts;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects;
using Edelstein.Protocol.Util.Buffers.Packets;

namespace Edelstein.Common.Gameplay.Stages.Game;

public class FieldSplit : AbstractFieldObjectPool, IFieldSplit
{
    private readonly IGameContextEvents _events;
    private readonly ICollection<IFieldObject> _objects;
    private readonly ICollection<IFieldSplitObserver> _observers;

    public FieldSplit(int row, int col, IGameContextEvents events)
    {
        Row = row;
        Col = col;
        _events = events;
        _objects = new List<IFieldObject>();
        _observers = new List<IFieldSplitObserver>();
    }

    public override IReadOnlyCollection<IFieldObject> Objects => _objects.ToImmutableList();
    public IReadOnlyCollection<IFieldSplitObserver> Observers => _observers.ToImmutableList();

    public int Row { get; }
    public int Col { get; }

    public override Task Enter(IFieldObject obj) => Enter(obj, null);
    public override Task Leave(IFieldObject obj) => Leave(obj, null);

    public async Task Enter(IFieldObject obj, Func<IPacket>? getEnterPacket, Func<IPacket>? getLeavePacket = null)
    {
        var from = obj.FieldSplit;

        if (from != null)
            await from.MigrateOut(obj);
        await MigrateIn(obj);

        obj.FieldSplit = this;

        var toObservers = Observers;
        var fromObservers = from?.Observers ?? new List<IFieldSplitObserver>();
        var newWatchers = toObservers
            .Where(w => w != obj)
            .Where(obj.IsVisibleTo)
            .Except(fromObservers)
            .ToImmutableList();
        var oldWatchers = fromObservers
            .Where(w => w != obj)
            .Where(obj.IsVisibleTo)
            .Except(toObservers)
            .ToImmutableList();

        var enterPacket = getEnterPacket?.Invoke() ?? obj.GetEnterFieldPacket();
        var leavePacket = getLeavePacket?.Invoke() ?? obj.GetLeaveFieldPacket();

        await Task.WhenAll(newWatchers.Select(w => w.Dispatch(enterPacket)));
        await Task.WhenAll(oldWatchers.Select(w => w.Dispatch(leavePacket)));

        if (obj is IFieldSplitObserver observer)
        {
            var enclosingSplits = observer.Field?.GetEnclosingSplits(this) ?? Array.Empty<IFieldSplit>();
            var oldSplits = observer.Observing
                .Except(enclosingSplits)
                .Where(s => s != null)
                .ToImmutableList();
            var newSplits = enclosingSplits
                .Except(observer.Observing)
                .Where(s => s != null)
                .ToImmutableList();

            await Task.WhenAll(oldSplits.Select(s => s!.Unobserve(observer)));
            await Task.WhenAll(newSplits.Select(s => s!.Observe(observer)));
        }

        await UpdateControllableObjects();

        await _events.ObjectEnterFieldSplit.Publish(new ObjectEnterFieldSplit(obj, this));
    }

    public async Task Leave(IFieldObject obj, Func<IPacket>? getLeavePacket)
    {
        obj.FieldSplit = null;

        await MigrateOut(obj);
        await Dispatch(getLeavePacket?.Invoke() ?? obj.GetLeaveFieldPacket(), obj);
        await UpdateControllableObjects();

        await _events.ObjectLeaveFieldSplit.Publish(new ObjectLeaveFieldSplit(obj, this));
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

        await Task.WhenAll(GetObjects()
            .Where(o => o != observer)
            .Where(o => o.IsVisibleTo(observer))
            .Select(o => observer.Dispatch(o.GetEnterFieldPacket())));
        await UpdateControllableObjects();
    }

    public async Task Unobserve(IFieldSplitObserver observer)
    {
        _observers.Remove(observer);
        observer.Observing.Remove(this);

        await Task.WhenAll(GetObjects()
            .Where(o => o != observer)
            .Where(o => o.IsVisibleTo(observer))
            .Select(o => observer.Dispatch(o.GetLeaveFieldPacket())));
        await UpdateControllableObjects();
    }

    public override Task Dispatch(IPacket packet) =>
        Task.WhenAll(Observers
            .Select(a => a.Dispatch(packet)));

    public override Task Dispatch(IPacket packet, IFieldObject obj) =>
        Task.WhenAll(Observers
            .Where(a => a != obj)
            .Where(obj.IsVisibleTo)
            .Select(a => a.Dispatch(packet)));

    public override IFieldObject? GetObject(int id) => Objects.FirstOrDefault(o => o.ObjectID == id);
    public override IEnumerable<IFieldObject> GetObjects() => Objects;

    private async Task UpdateControllableObjects()
    {
        var controllers = Observers
            .OfType<IFieldController>()
            .OrderBy(u => u.Controlled.Count)
            .ToImmutableList();
        var controllables = GetObjects()
            .OfType<IFieldControllable>()
            .ToImmutableList();

        await Task.WhenAll(controllables
            .Where(c => c.Controller == null || !controllers.Contains(c.Controller))
            .Select(c => c.Control(controllers.FirstOrDefault())));
    }
}
