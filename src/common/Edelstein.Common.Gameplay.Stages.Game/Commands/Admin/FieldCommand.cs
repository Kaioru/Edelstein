using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Edelstein.Common.Gameplay.Stages.Game.Commands.Util;
using Edelstein.Common.Gameplay.Stages.Game.Templates;
using Edelstein.Protocol.Gameplay.Stages.Game;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.User;
using Edelstein.Protocol.Gameplay.Templating;
using PowerArgs;

namespace Edelstein.Common.Gameplay.Stages.Game.Commands.Admin
{
    public class FieldCommand : AbstractTemplateCommand<FieldTemplate>
    {
        public override string Name => "Field";
        public override string Description => "Transfer to a specified field";

        private readonly IFieldRepository _repository;
        private readonly ITemplateRepository<FieldStringTemplate> _strings;

        public FieldCommand(
            IFieldRepository repository,
            ITemplateRepository<FieldStringTemplate> strings,
            ITemplateRepository<FieldTemplate> templates
        ) : base(templates)
        {
            _repository = repository;
            _strings = strings;

            Aliases.Add("Map");
            Aliases.Add("Warp");
        }

        protected override async Task<IEnumerable<TemplateCommandIndex>> Indeces()
        {
            var result = new List<TemplateCommandIndex>();
            var strings = (await _strings.RetrieveAll()).ToList();

            result.AddRange(strings.Select(s => new TemplateCommandIndex(s.ID, s.ID.ToString(), $"{s.StreetName}: {s.MapName}")));
            result.AddRange(strings.Select(s => new TemplateCommandIndex(s.ID, s.MapName, $"{s.StreetName}: {s.MapName}")));
            result.AddRange(strings.Select(s => new TemplateCommandIndex(s.ID, s.StreetName, $"{s.StreetName}: {s.MapName}")));

            return result;
        }

        protected override async Task Execute(IFieldObjUser user, FieldTemplate template, TemplateCommandArgs args)
        {
            var field = await _repository.Retrieve(template.ID);

            await field.Enter(user, 0);
        }
    }
}
