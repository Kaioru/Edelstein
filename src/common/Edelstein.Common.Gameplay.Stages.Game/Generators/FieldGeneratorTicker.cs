using Edelstein.Protocol.Gameplay.Stages.Game;
using Edelstein.Protocol.Util.Tickers;

namespace Edelstein.Common.Gameplay.Stages.Game.Generators;

public class FieldGeneratorTicker : ITickable
{
    private readonly IFieldManager _manager;

    public FieldGeneratorTicker(IFieldManager manager) => _manager = manager;

    public async Task OnTick(DateTime now)
    {
        var fields = await _manager.RetrieveAll();

        foreach (var field in fields)
        foreach (var objs in field.Generators.Select(g => g.Generate()))
        foreach (var obj in objs)
            await field.Enter(obj!);
    }
}
