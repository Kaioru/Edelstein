using System.Collections.Concurrent;
using System.Collections.Frozen;
using System.Collections.Immutable;
using Edelstein.Protocol.Gameplay.Game;
using Edelstein.Protocol.Gameplay.Game.Continents;
using Edelstein.Protocol.Utilities.Tickers;

namespace Edelstein.Common.Gameplay.Game.Continents;

public class ContiMoveManager : IContiMoveManager, ITickable
{
    private readonly IDictionary<int, IContiMove> _conti;
    private readonly ITickerManagerContext _tickerContext;

    public ContiMoveManager(ITickerManager tickerManager)
    {
        _conti = new ConcurrentDictionary<int, IContiMove>();
        _tickerContext = tickerManager.Schedule(this, TimeSpan.FromSeconds(30), TimeSpan.Zero);
    }

    public Task<IContiMove?> Retrieve(int key) =>
        Task.FromResult(_conti.TryGetValue(key, out var conti)
            ? conti
            : null);

    public Task<IContiMove?> RetrieveByName(string name) =>
        Task.FromResult(_conti.Values.FirstOrDefault(c => c.Template.Name == name));

    public Task<IContiMove?> RetrieveByField(IField field) =>
        Task.FromResult(_conti.Values.FirstOrDefault(c =>
            c.StartShipMoveField == field ||
            c.WaitField == field ||
            c.MoveField == field ||
            c.CabinField == field
        ));

    public Task<ICollection<IContiMove>> RetrieveAll() =>
        Task.FromResult<ICollection<IContiMove>>(_conti.Values.ToImmutableHashSet());

    public Task<IContiMove> Insert(IContiMove entry)
    {
        _conti.Add(entry.ID, entry);
        return Task.FromResult(entry);
    }

    public async Task OnTick(DateTime now)
        => await Task.WhenAll(_conti.Values.OfType<ITickable>().Select(c => c.OnTick(now)));
}
