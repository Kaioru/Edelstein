using System.Threading.Tasks;
using Edelstein.Common.Gameplay.Handling;
using Edelstein.Common.Gameplay.Social;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.User;
using Edelstein.Protocol.Network;
using Edelstein.Protocol.Services.Contracts;
using Edelstein.Protocol.Services.Contracts.Social;
using Google.Protobuf;
using Microsoft.Extensions.Logging;

namespace Edelstein.Common.Gameplay.Stages.Game.Objects.User.Handlers
{
    public class PartyResultHandler : AbstractUserPacketHandler
    {
        public override short Operation => (short)PacketRecvOperations.PartyResult;

        protected override async Task Handle(GameStageUser stageUser, IFieldObjUser user, IPacketReader packet)
        {
            var stage = stageUser.Stage;
            var service = stage.PartyService;
            var type = (PartyResultCode)packet.ReadByte();

            switch (type)
            {
                case PartyResultCode.InviteParty_Sent:
                case PartyResultCode.InviteParty_AlreadyInvited:
                case PartyResultCode.InviteParty_AlreadyInvitedByInviter:
                    break; // Do nothing
                case PartyResultCode.InviteParty_Rejected:
                    {
                        if (user.Party != null) return;

                        var serviceRequest = new InviteClaimRequest { Type = InviteType.Party, Invited = user.ID };
                        var serviceResponse = await stage.InviteService.Claim(serviceRequest);

                        if (serviceResponse.Result == InviteServiceResult.Ok)
                        {
                            var inviter = serviceResponse.Invite.Inviter;
                            var dispatch = new UnstructuredOutgoingPacket(PacketSendOperations.PartyResult)
                                .WriteByte((byte)PartyResultCode.ServerMsg)
                                .WriteBool(true)
                                .WriteString($"'{user.Character.Name}' rejected the party invitation request.");
                            var dispatchRequest = new DispatchToCharactersRequest { Data = ByteString.CopyFrom(dispatch.Buffer) };

                            dispatchRequest.Characters.Add(inviter);

                            await stage.DispatchService.DispatchToCharacters(dispatchRequest);
                        }
                        break;
                    }
                default:
                    stage.Logger.LogWarning($"Unhandled party result type: {type}");
                    break;
            }
        }
    }
}
