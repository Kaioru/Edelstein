using System;
using System.Linq;
using System.Threading.Tasks;
using Edelstein.Protocol.Gameplay.Contracts;
using Edelstein.Protocol.Gameplay.Game;
using Edelstein.Protocol.Gameplay.Game.Continents;
using Edelstein.Protocol.Gameplay.Game.Continents.Templates;
using Edelstein.Protocol.Utilities;
using Edelstein.Protocol.Utilities.Pipelines;
using Edelstein.Protocol.Utilities.Templates;
using Edelstein.Protocol.Utilities.Tickers;
using Microsoft.Extensions.Logging;

namespace Edelstein.Common.Gameplay.Game.Continent;

public class ContiMoveManagerInit(
    ILogger<ContiMove> logger,
    IDateTimeProvider dateTime,
    ITemplateManager<IContiMoveTemplate> templates,
    IContiMoveManager manager
) : IPipe<SystemOnStart<IGameStageSystem, IGameStageSystemUser>>
{
    public async Task Handle(IPipelineContext ctx, SystemOnStart<IGameStageSystem, IGameStageSystemUser> message)
    {
        await Task.WhenAll((await templates.RetrieveAll())
            .Select(t => new ContiMove(logger, dateTime, t))
            .Select(manager.Insert)
        );
    }
}
