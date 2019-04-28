using System.Linq;
using System.Threading.Tasks;
using Edelstein.Core;
using Edelstein.Core.Distributed.Peers.Info;
using Edelstein.Network.Packets;
using Edelstein.Service.Game.Conversations;
using Edelstein.Service.Game.Conversations.Speakers;
using Edelstein.Service.Game.Fields.Objects;

namespace Edelstein.Service.Game.Fields.User
{
    public partial class FieldUser
    {
        private async Task OnUserTransferFieldRequest(IPacket packet)
        {
            packet.Decode<byte>();
            packet.Decode<int>();

            var name = packet.Decode<string>();
            var portal = Field.GetPortal(name);

            await portal.Enter(this);
        }

        private async Task OnUserTransferChannelRequest(IPacket packet)
        {
            var channel = packet.Decode<byte>();

            try
            {
                var service = Socket.Service.Peers
                    .OfType<GameServiceInfo>()
                    .Where(g => g.WorldID == Socket.Service.Info.WorldID)
                    .OrderBy(g => g.ID)
                    .ToList()[channel];

                await Socket.TryMigrateTo(Socket.Account, Socket.Character, service);
            }
            catch
            {
                using (var p = new Packet(SendPacketOperations.TransferChannelReqIgnored))
                {
                    p.Encode<byte>(0x1);
                    await SendPacket(p);
                }
            }
        }

        private async Task OnUserMove(IPacket packet)
        {
            packet.Decode<long>();
            packet.Decode<byte>();
            packet.Decode<long>();
            packet.Decode<int>();
            packet.Decode<int>();
            packet.Decode<int>();

            var path = Move(packet);

            using (var p = new Packet(SendPacketOperations.UserMove))
            {
                p.Encode<int>(ID);
                path.Encode(p);
                await Field.BroadcastPacket(this, p);
            }
        }

        private async Task OnUserChat(IPacket packet)
        {
            packet.Decode<int>();

            var message = packet.Decode<string>();
            var onlyBalloon = packet.Decode<bool>();

            using (var p = new Packet(SendPacketOperations.UserChat))
            {
                p.Encode<int>(ID);
                p.Encode<bool>(false);
                p.Encode<string>(message);
                p.Encode<bool>(onlyBalloon);
                await Field.BroadcastPacket(p);
            }
        }

        private async Task OnUserEmotion(IPacket packet)
        {
            var emotion = packet.Decode<int>();
            var duration = packet.Decode<int>();
            var byItemOption = packet.Decode<bool>();

            // TODO: item option checks

            using (var p = new Packet(SendPacketOperations.UserEmotion))
            {
                p.Encode<int>(ID);
                p.Encode<int>(emotion);
                p.Encode<int>(duration);
                p.Encode<bool>(byItemOption);
                await Field.BroadcastPacket(this, p);
            }
        }

        private async Task OnUserSelectNPC(IPacket packet)
        {
            var npc = Field.GetObject<FieldNPC>(packet.Decode<int>());

            if (npc == null) return;

            var template = npc.Template;
            var script = template.Scripts.FirstOrDefault()?.Script;

            if (script == null) return;

            var context = new ConversationContext(Socket);
            var conversation = await Service.ConversationManager.Build(
                script,
                context,
                new FieldNPCSpeaker(context),
                new FieldUserSpeaker(context)
            );

            await Converse(conversation);
        }

        private async Task OnUserScriptMessageAnswer(IPacket packet)
        {
            if (ConversationContext == null) return;

            var type = (ConversationMessageType) packet.Decode<byte>();

            if (type != ConversationContext.LastRequestType) return;
            if (type == ConversationMessageType.AskQuiz ||
                type == ConversationMessageType.AskSpeedQuiz)
            {
                await ConversationContext.Respond(packet.Decode<string>());
                return;
            }

            var answer = packet.Decode<byte>();

            if (
                type != ConversationMessageType.Say &&
                type != ConversationMessageType.AskYesNo &&
                type != ConversationMessageType.AskAccept &&
                answer == byte.MinValue ||
                (type == ConversationMessageType.Say ||
                 type == ConversationMessageType.AskYesNo ||
                 type == ConversationMessageType.AskAccept) && answer == byte.MaxValue
            )
            {
                ConversationContext.TokenSource.Cancel();
                return;
            }

            switch (type)
            {
                case ConversationMessageType.AskText:
                case ConversationMessageType.AskBoxText:
                    await ConversationContext.Respond(packet.Decode<string>());
                    break;
                case ConversationMessageType.AskNumber:
                case ConversationMessageType.AskMenu:
                case ConversationMessageType.AskSlideMenu:
                    await ConversationContext.Respond(packet.Decode<int>());
                    break;
                case ConversationMessageType.AskAvatar:
                case ConversationMessageType.AskMemberShopAvatar:
                    await ConversationContext.Respond(packet.Decode<byte>());
                    break;
                default:
                    await ConversationContext.Respond(answer);
                    break;
            }
        }
    }
}