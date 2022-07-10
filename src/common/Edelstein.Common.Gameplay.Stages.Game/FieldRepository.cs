using System;
using System.Linq;
using System.Threading.Tasks;
using Edelstein.Common.Util.Repositories;
using Edelstein.Protocol.Gameplay.Stages.Game;
using Edelstein.Common.Gameplay.Stages.Game.Templates;
using Edelstein.Common.Gameplay.Templating;
using Edelstein.Protocol.Util.Ticks;

namespace Edelstein.Common.Gameplay.Stages.Game
{
    public class FieldRepository : LocalRepository<int, IField>, IFieldRepository, ITickerBehavior
    {
        private readonly GameStage _gameStage;
        private readonly ITemplateRepository<FieldTemplate> _fieldTemplates;

        public FieldRepository(
            GameStage gameStage,
            ITemplateRepository<FieldTemplate> fieldTemplates,
            ITickerManager tickerManager
        )
        {
            _gameStage = gameStage;
            _fieldTemplates = fieldTemplates;

            tickerManager.Schedule(this, TimeSpan.FromSeconds(1));
        }

        public async Task OnTick(DateTime now)
            => await Task.WhenAll((await RetrieveAll()).OfType<ITickerBehavior>().Select(f => f.OnTick(now)));

        public override async Task<IField> Retrieve(int key)
        {
            var field = await base.Retrieve(key);
            if (field != null) return field;
            var template = await _fieldTemplates.Retrieve(key);
            if (template == null) return null;
            field = new Field(_gameStage, template);
            await Insert(field);
            return field;
        }
    }
}
