using System.Linq;
using System.Threading.Tasks;
using Edelstein.Core;
using Edelstein.Network.Packets;
using Edelstein.Service.Game.Conversations;
using Edelstein.Service.Game.Conversations.Speakers.Fields;
using Edelstein.Service.Game.Fields.Objects.User;

namespace Edelstein.Service.Game.Services.Handlers.User
{
    public class UserPortalScriptRequestHandler : AbstractFieldUserHandler
    {
        public override async Task Handle(RecvPacketOperations operation, IPacket packet, FieldUser user)
        {
            packet.Decode<byte>();

            var name = packet.Decode<string>();
            var portal = user.Field.Template.Portals.Values
                .FirstOrDefault(p => p.Name.Equals(name));

            if (portal == null) return;
            if (string.IsNullOrEmpty(portal.Script)) return;

            var context = new ConversationContext(user.Socket);
            var conversation = await user.Service.ConversationManager.Build(
                portal.Script,
                context,
                new FieldSpeaker(context, user.Field),
                new FieldUserSpeaker(context, user)
            );

            await user.Converse(conversation);
        }
    }
}