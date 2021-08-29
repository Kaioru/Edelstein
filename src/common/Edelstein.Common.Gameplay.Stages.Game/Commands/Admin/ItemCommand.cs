using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Edelstein.Common.Gameplay.Stages.Game.Commands.Util;
using Edelstein.Common.Gameplay.Users.Inventories.Templates;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.User;
using Edelstein.Protocol.Gameplay.Templating;

namespace Edelstein.Common.Gameplay.Stages.Game.Commands.Admin
{
    public class ItemCommand : AbstractTemplateCommand<ItemTemplate>
    {
        public override string Name => "Item";
        public override string Description => "Creates a specified item";

        private readonly ITemplateRepository<ItemStringTemplate> _strings;

        public ItemCommand(
            ITemplateRepository<ItemStringTemplate> strings,
            ITemplateRepository<ItemTemplate> templates
        ) : base(templates)
        {
            _strings = strings;

            Aliases.Add("Create");
        }

        protected override async Task<IEnumerable<TemplateCommandIndex>> Indices()
        {
            var result = new List<TemplateCommandIndex>();
            var strings = (await _strings.RetrieveAll()).ToList();

            result.AddRange(strings.Select(s => new TemplateCommandIndex(s.ID, s.ID.ToString(), s.Name)));
            result.AddRange(strings.Select(s => new TemplateCommandIndex(s.ID, s.Name, s.Name)));

            return result;
        }

        protected override async Task Execute(IFieldObjUser user, ItemTemplate template, TemplateCommandArgs args)
        {
            await user.ModifyInventory(i => i.Add(template.ID));
        }
    }
}