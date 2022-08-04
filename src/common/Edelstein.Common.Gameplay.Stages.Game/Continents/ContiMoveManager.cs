using System.Collections.Concurrent;
using Edelstein.Protocol.Gameplay.Stages.Game;
using Edelstein.Protocol.Gameplay.Stages.Game.Continents;
using Edelstein.Protocol.Util.Tickers;

namespace Edelstein.Common.Gameplay.Stages.Game.Continents;

public class ContiMoveManager : IContiMoveManager, ITickable
{
    private readonly IDictionary<int, IContiMove> _conti;
    private readonly ITickerManagerContext _tickerContext;

    public ContiMoveManager(ITickerManager tickerManager)
    {
        _conti = new ConcurrentDictionary<int, IContiMove>();
        _tickerContext = tickerManager.Schedule(this, TimeSpan.FromSeconds(30));
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

    public Task<IContiMove> Insert(IContiMove entry)
    {
        _conti.Add(entry.ID, entry);
        return Task.FromResult(entry);
    }

    public async Task OnTick(DateTime now)
        => await Task.WhenAll(_conti.Values.OfType<ITickable>().Select(c => c.OnTick(now)));
}
