using System;
using System.Threading.Tasks;
using CommandLine;
using Edelstein.Provider.Templates.Item;
using Edelstein.Provider.Templates.String;
using Edelstein.Service.Game.Fields.User;

namespace Edelstein.Service.Game.Commands.Handling
{
    public class ItemCommand : AbstractTemplateCommand<ItemTemplate, ItemStringTemplate, ItemCommandOption>
    {
        public override string Name => "Item";
        public override string Description => "Creates an item from the specified options";

        public ItemCommand(Parser parser) : base(parser)
        {
        }

        protected override async Task ExecuteAfter(
            FieldUser sender,
            ItemTemplate template,
            ItemCommandOption option
        )
        {
            await sender.ModifyInventory(i => i.Add(template, option.Quantity ?? 1));
        }
    }

    public class ItemCommandOption : TemplateCommandOption
    {
        [Option('q', "quantity", HelpText = "The item quantity.")]
        public short? Quantity { get; set; }
    }
}