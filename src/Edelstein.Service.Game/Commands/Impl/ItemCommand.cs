using System.Threading.Tasks;
using CommandLine;
using Edelstein.Core.Templates.Items;
using Edelstein.Core.Templates.Strings;
using Edelstein.Service.Game.Commands.Util;
using Edelstein.Service.Game.Fields.Objects.User;

namespace Edelstein.Service.Game.Commands.Impl
{
    public class ItemCommand : AbstractTemplateCommand<
        ItemTemplate,
        ItemStringTemplate,
        ItemCommandOption
    >
    {
        public override string Name => "Item";
        public override string Description => "Creates an item from the specified options";

        public ItemCommand(Parser parser) : base(parser)
        {
            Aliases.Add("Create");
        }

        protected override async Task Run(
            FieldUser sender,
            ItemTemplate template,
            ItemCommandOption ctx
        )
        {
            await sender.ModifyInventory(i => i.Add(template, ctx.Quantity ?? 1));
        }
    }

    public class ItemCommandOption : DefaultTemplateCommandContext
    {
        [Option('q', "quantity", HelpText = "The item quantity.")]
        public short? Quantity { get; set; }
    }
}