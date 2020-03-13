using System.Linq;
using System.Threading.Tasks;
using Edelstein.Core.Utils;
using Edelstein.Network.Packets;
using Edelstein.Service.Game.Conversations;
using Edelstein.Service.Game.Conversations.Speakers.Field;
using Edelstein.Service.Game.Fields.Objects.NPC;
using Edelstein.Service.Game.Fields.Objects.User;

namespace Edelstein.Service.Game.Handlers.Users
{
    public class UserSelectNPCHandler : AbstractFieldUserHandler
    {
        protected override async Task Handle(
            FieldUser user,
            RecvPacketOperations operation,
            IPacket packet
        )
        {
            var npc = user.GetWatchedObject<FieldNPC>(packet.Decode<int>());
            var script = npc.Template.Scripts.FirstOrDefault()?.Script;

            if (script == null) return;

            var context = new ConversationContext(user.Adapter.Socket);
            var conversation = await user.Service.ConversationManager.Build(
                script,
                context,
                new FieldNPCSpeaker(context, npc),
                new FieldUserSpeaker(context, user)
            );

            await user.Converse(context, conversation);
        }
    }
}