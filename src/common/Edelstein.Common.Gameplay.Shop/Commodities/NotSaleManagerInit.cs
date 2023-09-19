using Edelstein.Common.Gameplay.Shop.Commodities.Templates;
using Edelstein.Protocol.Gameplay.Contracts;
using Edelstein.Protocol.Gameplay.Shop.Commodities;
using Edelstein.Protocol.Utilities.Pipelines;
using Edelstein.Protocol.Utilities.Templates;

namespace Edelstein.Common.Gameplay.Shop.Commodities;

public class NotSaleManagerInit : IPipelinePlug<StageStart>
{
    private readonly INotSaleManager _manager;
    private readonly ITemplateManager<NotSaleTemplate> _templates;
    
    public NotSaleManagerInit(INotSaleManager manager, ITemplateManager<NotSaleTemplate> templates)
    {
        _manager = manager;
        _templates = templates;
    }

    public async Task Handle(IPipelineContext ctx, StageStart message) 
        => await Task.WhenAll((await _templates.RetrieveAll()).Select(_manager.Insert));
}
