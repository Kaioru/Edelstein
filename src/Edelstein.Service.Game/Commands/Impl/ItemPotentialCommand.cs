using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommandLine;
using Edelstein.Entities.Inventories;
using Edelstein.Entities.Inventories.Items;
using Edelstein.Service.Game.Fields.Objects.User;
using IronPython.Compiler.Ast;
using Microsoft.Scripting.Utils;
using MoreLinq;

namespace Edelstein.Service.Game.Commands.Impl
{
    public class ItemPotentialCommand : AbstractCommand<DefaultCommandContext>
    {
        public override string Name => "ItemPotential";
        public override string Description => "Reroll a current equipment's potential stats";

        public ItemPotentialCommand(Parser parser) : base(parser)
        {
            Aliases.Add("Potential");
        }

        protected override async Task Run(FieldUser sender, DefaultCommandContext ctx)
        {
            var items = sender.Character.Inventories[ItemInventoryType.Equip].Items;
            var slot = await sender.Prompt<int>(s => s.AskMenu(
                "Which equipment would you like to reroll?",
                items.ToDictionary(
                    kv => (int) kv.Key,
                    kv => $"#i{kv.Value.TemplateID}# #z{kv.Value.TemplateID}#"
                )
            ));

            if (!(items[(short) slot] is ItemSlotEquip equip)) return;

            var type = (ItemOptionUnreleaseType) await sender.Prompt<int>(s => s.AskMenu(
                "Which re-roll type?",
                Enum.GetValues(typeof(ItemOptionUnreleaseType))
                    .Select(t => (int) t)
                    .ToDictionary(
                        v => v,
                        v => ((ItemOptionUnreleaseType) v).ToString()
                    )
            ));

            await sender.UnreleaseItemOption(equip, type: type);
            await sender.ReleaseItemOption(equip);
        }
    }
}