using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommandLine;
using Edelstein.Entities.Inventories;
using Edelstein.Entities.Inventories.Items;
using Edelstein.Service.Game.Fields.Objects.User;
using IronPython.Compiler.Ast;

namespace Edelstein.Service.Game.Commands.Impl
{
    public class ItemPotentialCommand : AbstractCommand<DefaultCommandContext>
    {
        public override string Name => "ItemPotential";
        public override string Description => "Edit a current equipment's potential stats";

        public ItemPotentialCommand(Parser parser) : base(parser)
        {
            Aliases.Add("Potential");
        }

        protected override async Task Run(FieldUser sender, DefaultCommandContext ctx)
        {
            var items = sender.Character.Inventories[ItemInventoryType.Equip].Items;
            var slot = await sender.Prompt<int>(s => s.AskMenu(
                "Which equipment would you like to edit?",
                items.ToDictionary(
                    kv => (int) kv.Key,
                    kv => $"#i{kv.Value.TemplateID}# #z{kv.Value.TemplateID}#"
                )
            ));

            if (!(items[(short) slot] is ItemSlotEquip item)) return;

            var selection = await sender.Prompt<int>(s => s.AskMenu(
                "Which stat would you like to change?",
                new Dictionary<int, string>()
                {
                    [0] = "Grade",
                    [1] = "Option 1",
                    [2] = "Option 2",
                    [3] = "Option 3"
                }
            ));
            var value = await sender.Prompt<int>(s => s.AskNumber("What value would you like to set it to?"));

            switch (selection)
            {
                case 0:
                    item.Grade = (byte) value;
                    break;
                case 1:
                    item.Option1 = (short) value;
                    break;
                case 2:
                    item.Option2 = (short) value;
                    break;
                case 3:
                    item.Option3 = (short) value;
                    break;
            }

            await sender.ModifyInventory(i => i.Update(item));
        }
    }
}