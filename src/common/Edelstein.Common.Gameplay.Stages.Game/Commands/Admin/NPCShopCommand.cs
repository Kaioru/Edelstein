using System.Threading.Tasks;
using Edelstein.Common.Gameplay.Stages.Game.Dialogs;
using Edelstein.Common.Gameplay.Stages.Game.Dialogs.Templates;
using Edelstein.Common.Gameplay.Templating;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.User;
using PowerArgs;

namespace Edelstein.Common.Gameplay.Stages.Game.Commands.Admin
{
    public class NPCShopCommandArgs : CommandArgs
    {
        [ArgPosition(0), ArgRequired]
        [ArgDescription("The template ID of the NPC")]
        public int TemplateID { get; set; }
    }

    public class NPCShopCommand : AbstractCommand<NPCShopCommandArgs>
    {
        private readonly ITemplateRepository<NPCShopTemplate> _templates;

        public override string Name => "NPCShop";
        public override string Description => "Opens an NPC Shop with the specified NPC";

        public NPCShopCommand(ITemplateRepository<NPCShopTemplate> templates)
        {
            _templates = templates;

            Aliases.Add("Shop");
        }

        public override async Task Execute(IFieldObjUser user, NPCShopCommandArgs args)
        {
            var template = await _templates.Retrieve(args.TemplateID);

            if (template == null)
            {
                await user.Message($"The specified NPC Shop: {args.TemplateID}, does not exist.");
                return;
            }

            var shop = new ShopDialog(template.ID, template);

            await user.Dialog(shop);
        }
    }
}
