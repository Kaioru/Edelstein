using System;
using System.Linq;
using System.Threading.Tasks;
using Edelstein.Common.Gameplay.Handling;
using Edelstein.Common.Gameplay.Stages.Game.Objects.User.Types;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.User;
using Edelstein.Protocol.Network;
using Edelstein.Protocol.Services.Contracts;
using Google.Protobuf;
using Microsoft.Extensions.Logging;

namespace Edelstein.Common.Gameplay.Stages.Game.Objects.User.Handlers
{
    public class GroupMessageHandler : AbstractUserPacketHandler
    {
        public override short Operation => (short)PacketRecvOperations.GroupMessage;

        protected override async Task Handle(GameStageUser stageUser, IFieldObjUser user, IPacketReader packet)
        {
            _ = packet.ReadInt();

            var type = (GroupMessageType)packet.ReadByte();

            var recipientCount = packet.ReadByte();
            var recipients = new int[recipientCount];

            for (var i = 0; i < recipientCount; i++)
                recipients[i] = packet.ReadInt();

            var text = packet.ReadString();

            switch (type)
            {
                case GroupMessageType.Party:
                    {
                        if (user.Party == null) return;

                        var partyChat = new UnstructuredOutgoingPacket(PacketSendOperations.GroupMessage);

                        partyChat.WriteByte((byte)GroupMessageType.Party);
                        partyChat.WriteString(user.Character.Name);
                        partyChat.WriteString(text);

                        var dispatchRequest = new DispatchToCharactersRequest { Data = ByteString.CopyFrom(partyChat.Buffer) };

                        dispatchRequest.Characters.Add(user.Party.Members
                            .Select(m => m.ID)
                            .Where(m => m != user.ID)
                        );

                        await stageUser.Stage.DispatchService.DispatchToCharacters(dispatchRequest);
                        break;
                    }
                default:
                    stageUser.Stage.Logger.LogWarning($"Unhandled group message type: {type}");
                    break;
            }
        }
    }
}
