using System.Linq;
using System.Threading.Tasks;
using Edelstein.Common.Gameplay.Handling;
using Edelstein.Common.Gameplay.Social;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.User;
using Edelstein.Protocol.Network;
using Edelstein.Protocol.Services.Contracts;
using Edelstein.Protocol.Services.Contracts.Social;
using Edelstein.Protocol.Util.Spatial;
using Google.Protobuf;
using Microsoft.Extensions.Logging;

namespace Edelstein.Common.Gameplay.Stages.Game.Objects.User.Handlers
{
    public class PartyRequestHandler : AbstractUserPacketHandler
    {
        public override short Operation => (short)PacketRecvOperations.PartyRequest;

        protected override async Task Handle(GameStageUser stageUser, IFieldObjUser user, IPacketReader packet)
        {
            var stage = stageUser.Stage;
            var service = stage.PartyService;
            var type = (PartyRequestCode)packet.ReadByte();

            switch (type)
            {
                case PartyRequestCode.CreateNewParty:
                    {
                        if (user.Party != null) return;

                        var result = PartyResultCode.CreateNewParty_Done;
                        var response = new UnstructuredOutgoingPacket(PacketSendOperations.PartyResult);
                        var contract = new PartyCreateRequest
                        {
                            Member = new PartyMemberContract
                            {
                                Id = user.ID,
                                Name = user.Character.Name,
                                Job = user.Character.Job,
                                Level = user.Character.Level,
                                Channel = stage.ChannelID,
                                Field = user.Field.ID
                            }
                        };
                        var serviceResponse = await service.Create(contract);
                        var serviceResult = serviceResponse.Result;

                        if (serviceResult == PartyServiceResult.FailedAlreadyInParty) result = PartyResultCode.CreateNewParty_AlreadyJoined;
                        else if (serviceResult != PartyServiceResult.Ok) result = PartyResultCode.CreateNewParty_Unknown;

                        response.WriteByte((byte)result);

                        if (result == PartyResultCode.CreateNewParty_Done)
                        {
                            response.WriteInt(serviceResponse.Party.Id);
                            response.WriteInt(0); // TownPortal-TownID
                            response.WriteInt(0); // TownPortal-FieldID
                            response.WriteInt(0); // TownPortal-SkillID
                            response.WritePoint2D(new Point2D(0, 0)); //TownPortal-Position
                        }

                        await user.Dispatch(response);
                        break;
                    }
                case PartyRequestCode.WithdrawParty:
                    {
                        if (user.Party == null) return;

                        var result = PartyResultCode.WithdrawParty_Done;
                        var response = new UnstructuredOutgoingPacket(PacketSendOperations.PartyResult);
                        var contract = new PartyWithdrawRequest { Character = user.ID, IsKick = false };
                        var serviceResponse = await service.Withdraw(contract);
                        var serviceResult = serviceResponse.Result;

                        if (serviceResult == PartyServiceResult.FailedNotInParty) result = PartyResultCode.WithdrawParty_NotJoined;
                        else if (serviceResult != PartyServiceResult.Ok) result = PartyResultCode.WithdrawParty_Unknown;

                        response.WriteByte((byte)result);

                        if (result == PartyResultCode.WithdrawParty_Done) return;

                        await user.Dispatch(response);
                        break;
                    }
                case PartyRequestCode.InviteParty:
                    {
                        if (user.Party == null)
                        {
                            var contract = new PartyCreateRequest
                            {
                                Member = new PartyMemberContract
                                {
                                    Id = user.ID,
                                    Name = user.Character.Name,
                                    Job = user.Character.Job,
                                    Level = user.Character.Level,
                                    Channel = stage.ChannelID,
                                    Field = user.Field.ID
                                }
                            };
                            var serviceResponse = await service.Create(contract);
                            var serviceResult = serviceResponse.Result;

                            if (serviceResult != PartyServiceResult.Ok) return;

                            var createResponse = new UnstructuredOutgoingPacket(PacketSendOperations.PartyResult);

                            createResponse.WriteByte((byte)PartyResultCode.CreateNewParty_Done);
                            createResponse.WriteInt(serviceResponse.Party.Id);
                            createResponse.WriteInt(0); // TownPortal-TownID
                            createResponse.WriteInt(0); // TownPortal-FieldID
                            createResponse.WriteInt(0); // TownPortal-SkillID
                            createResponse.WritePoint2D(new Point2D(0, 0)); //TownPortal-Position

                            await user.Dispatch(createResponse);
                        }

                        if (user?.Party?.Boss != user.ID) return;

                        var name = packet.ReadString();
                        var result = PartyResultCode.InviteParty_Sent;
                        var response = new UnstructuredOutgoingPacket(PacketSendOperations.PartyResult);
                        var character = await stage.CharacterRepository.RetrieveByName(name);

                        if (character == null || user.Character.ID == character.ID) result = PartyResultCode.InviteParty_BlockedUser;
                        if ((await service.LoadByCharacter(new PartyLoadByCharacterRequest { Character = character.ID })).Party != null) result = PartyResultCode.JoinParty_AlreadyJoined;
                        else
                        {
                            var serviceResponse = await stage.InviteService.Register(new InviteRegisterRequest
                            {
                                Invite = new InviteContract
                                {
                                    Type = InviteType.Party,
                                    Invited = character.ID,
                                    Inviter = user.ID
                                }
                            });

                            if (serviceResponse.Result != InviteServiceResult.Ok) result = PartyResultCode.InviteParty_AlreadyInvited;
                        }

                        response.WriteByte((byte)result);

                        if (result == PartyResultCode.InviteParty_Sent)
                        {
                            var invitation = new UnstructuredOutgoingPacket(PacketSendOperations.PartyResult);
                            var invitationRequest = new DispatchToCharactersRequest();

                            invitation.WriteByte((byte)PartyRequestCode.InviteParty);
                            invitation.WriteInt(user.Character.ID);
                            invitation.WriteString(user.Character.Name);
                            invitation.WriteInt(user.Character.Level);
                            invitation.WriteInt(user.Character.Job);
                            invitation.WriteByte(0); // PartyOpt

                            invitationRequest.Data = ByteString.CopyFrom(invitation.Buffer);
                            invitationRequest.Characters.Add(character.ID);

                            await stage.DispatchService.DispatchToCharacters(invitationRequest);

                            response.WriteString(character.Name);
                        }

                        await user.Dispatch(response);
                        break;
                    }
                case PartyRequestCode.KickParty:
                    {
                        if (user.Party == null) return;
                        if (user.Party.Boss != user.ID) return;

                        var id = packet.ReadInt();

                        if (!user.Party.Members.Any(m => m.ID == id)) return;
                        if (id == user.ID) return;

                        var result = PartyResultCode.WithdrawParty_Done;
                        var response = new UnstructuredOutgoingPacket(PacketSendOperations.PartyResult);
                        var contract = new PartyWithdrawRequest { Character = id, IsKick = true };
                        var serviceResponse = await service.Withdraw(contract);
                        var serviceResult = serviceResponse.Result;

                        if (serviceResult == PartyServiceResult.FailedNotInParty) result = PartyResultCode.WithdrawParty_NotJoined;
                        else if (serviceResult != PartyServiceResult.Ok) result = PartyResultCode.WithdrawParty_Unknown;

                        response.WriteByte((byte)result);

                        if (result == PartyResultCode.WithdrawParty_Done) return;

                        await user.Dispatch(response);
                        break;
                    }
                case PartyRequestCode.ChangePartyBoss:
                    {
                        if (user.Party == null || user.Party.Boss != user.ID) return;

                        var id = packet.ReadInt();

                        if (!user.Party.Members.Any(m => m.ID == id)) return;
                        if (id == user.ID) return;

                        var result = PartyResultCode.ChangePartyBoss_Done;
                        var response = new UnstructuredOutgoingPacket(PacketSendOperations.PartyResult);
                        var contract = new PartyChangeBossRequest { Character = id };
                        var serviceResponse = await service.ChangeBoss(contract);
                        var serviceResult = serviceResponse.Result;

                        if (serviceResult != PartyServiceResult.Ok) result = PartyResultCode.WithdrawParty_Unknown;

                        response.WriteByte((byte)result);

                        if (result == PartyResultCode.ChangePartyBoss_Done) return;

                        await user.Dispatch(response);
                        break;
                    }
                default:
                    stage.Logger.LogWarning($"Unhandled party request type: {type}");
                    break;
            }
        }
    }
}
