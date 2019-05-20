using System.Linq;
using System.Threading.Tasks;
using Edelstein.Core;
using Edelstein.Network.Packets;
using Edelstein.Provider.Templates.Etc.NPCShop;
using Edelstein.Service.Game.Conversations;
using Edelstein.Service.Game.Conversations.Speakers.Fields;
using Edelstein.Service.Game.Fields.Objects.NPC;
using Edelstein.Service.Game.Fields.Objects.User;
using Edelstein.Service.Game.Interactions;

namespace Edelstein.Service.Game.Services.Handlers.User
{
    public class UserSelectNPCHandler : AbstractFieldUserHandler
    {
        public override async Task Handle(RecvPacketOperations operation, IPacket packet, FieldUser user)
        {
            var npc = user.Field.GetObject<FieldNPC>(packet.Decode<int>());

            if (npc == null) return;

            var template = npc.Template;
            var shop = user.Service.TemplateManager.Get<NPCShopTemplate>(template.ID);

            if (shop != null)
            {
                await user.Interact(new ShopDialog(user, shop));
                return;
            }

            var script = template.Scripts.FirstOrDefault()?.Script;

            if (script == null) return;

            var context = new ConversationContext(user.Socket);
            var conversation = await user.Service.ConversationManager.Build(
                script,
                context,
                new FieldNPCSpeaker(context, npc),
                new FieldUserSpeaker(context, user)
            );

            await user.Converse(conversation);
        }
    }
}