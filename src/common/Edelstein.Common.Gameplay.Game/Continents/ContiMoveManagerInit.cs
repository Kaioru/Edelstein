using Edelstein.Common.Utilities.Tickers;
using Edelstein.Protocol.Gameplay.Contracts;
using Edelstein.Protocol.Gameplay.Game;
using Edelstein.Protocol.Gameplay.Game.Continents;
using Edelstein.Protocol.Gameplay.Game.Continents.Templates;
using Edelstein.Protocol.Utilities.Pipelines;
using Edelstein.Protocol.Utilities.Templates;
using Edelstein.Protocol.Utilities.Tickers;
using Microsoft.Extensions.Logging;

namespace Edelstein.Common.Gameplay.Game.Continents;

public class ContiMoveManagerInit : IPipelinePlug<StageStart>
{
    private readonly ILogger<ContiMove> _logger;
    private readonly IContiMoveManager _contiMoveManager;
    private readonly IFieldManager _fieldManager;
    private readonly ITemplateManager<IContiMoveTemplate> _templates;

    public ContiMoveManagerInit(
        ILogger<ContiMove> logger, 
        IContiMoveManager contiMoveManager, 
        IFieldManager fieldManager, 
        ITemplateManager<IContiMoveTemplate> templates
    )
    {
        _logger = logger;
        _contiMoveManager = contiMoveManager;
        _fieldManager = fieldManager;
        _templates = templates;
    }
    
    private TickerManagerContext? Context { get; set; }

    public async Task Handle(IPipelineContext ctx, StageStart message)
    {
        await Task.WhenAll((await _templates.RetrieveAll())
            .Select(t => new ContiMove(_logger, _fieldManager, t))
            .Select(_contiMoveManager.Insert)
        );
    }
}
