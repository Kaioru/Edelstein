using System.Threading.Tasks;
using CommandLine;
using Edelstein.Provider.Templates.Item;
using Edelstein.Service.Game.Field.User;

namespace Edelstein.Service.Game.Commands.Handling
{
    public class ItemCommand : AbstractTemplateCommand<ItemTemplate>
    {
        public override string Name => "Item";
        public override string Description => "Creates an item from the specified options";

        public ItemCommand(Parser parser) : base(parser)
        {
        }

        protected override Task ExecuteAfter(FieldUser user, ItemTemplate template, TemplateCommandOption option)
            => user.ModifyInventory(i => i.Add(template));
    }
}