using System;
using System.Linq;
using System.Threading.Tasks;
using Edelstein.Common.Gameplay.Stages.Game.Continent.Templates;
using Edelstein.Common.Util.Repositories;
using Edelstein.Protocol.Gameplay.Stages;
using Edelstein.Protocol.Gameplay.Stages.Game;
using Edelstein.Protocol.Gameplay.Stages.Game.Continent;
using Edelstein.Protocol.Gameplay.Templating;
using Edelstein.Protocol.Util.Ticks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Edelstein.Common.Gameplay.Stages.Game.Continent
{
    public class ContiMoveRepository : LocalRepository<int, IContiMove>, IContiMoveRepository, ITickerBehavior
    {
        public ContiMoveRepository(
            GameStage gameStage,
            ITemplateRepository<ContiMoveTemplate> templates,
            IFieldRepository fieldRepository,
            ITickerManager tickerManager
        )
        {
            Task.WhenAll(templates.RetrieveAll().Result
                .Select(t => new ContiMove(gameStage, t, fieldRepository))
                .Select(c => Insert(c))
            ).Wait();

            tickerManager.Schedule(this, TimeSpan.FromSeconds(30));
        }

        public async Task<IContiMove> RetrieveByField(IField field)
            => (await RetrieveAll()).FirstOrDefault(c =>
                c.StartShipMoveField == field ||
                c.WaitField == field ||
                c.MoveField == field ||
                c.CabinField == field
            );

        public async Task<IContiMove> RetrieveByName(string name)
            => (await RetrieveAll()).FirstOrDefault(c => c.Info.Name.Equals(name));

        public async Task OnTick(DateTime now)
            => await Task.WhenAll((await RetrieveAll()).OfType<ITickerBehavior>().Select(c => c.OnTick(now)));
    }
}
