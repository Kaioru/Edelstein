using Edelstein.Common.Utilities.Tickers;
using Edelstein.Protocol.Gameplay.Contracts;
using Edelstein.Protocol.Gameplay.Game;
using Edelstein.Protocol.Utilities.Pipelines;
using Edelstein.Protocol.Utilities.Tickers;

namespace Edelstein.Common.Gameplay.Game;

public class FieldManagerInit : IPipelinePlug<StageStart>, ITickable
{
    private readonly IFieldManager _fieldManager;
    private readonly ITickerManager _tickerManager;

    public FieldManagerInit(IFieldManager fieldManager, ITickerManager tickerManager)
    {
        _fieldManager = fieldManager;
        _tickerManager = tickerManager;
    }

    private ITickerManagerContext? Context { get; set; }
    
    public Task Handle(IPipelineContext ctx, StageStart message)
    {
        Context = _tickerManager.Schedule(this, TimeSpan.FromSeconds(1));
        return Task.CompletedTask;
    }
    
    public async Task OnTick(DateTime now)
    {
        var fields = await _fieldManager.RetrieveAll();
        await Task.WhenAll(fields
            .OfType<ITickable>()
            .Select(f => f.OnTick(now)));
    }
}
