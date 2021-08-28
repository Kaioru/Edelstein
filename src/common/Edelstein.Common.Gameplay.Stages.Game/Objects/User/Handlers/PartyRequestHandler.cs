using System.Threading.Tasks;
using Edelstein.Common.Gameplay.Handling;
using Edelstein.Common.Gameplay.Social;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.User;
using Edelstein.Protocol.Network;
using Edelstein.Protocol.Services.Contracts.Social;
using Edelstein.Protocol.Util.Spatial;
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
                        var party = user.Party;
                        var result = PartyResultCode.WithdrawParty_Done;
                        var response = new UnstructuredOutgoingPacket(PacketSendOperations.PartyResult);
                        var contract = new PartyWithdrawRequest { Character = user.ID };
                        var serviceResponse = await service.Withdraw(contract);
                        var serviceResult = serviceResponse.Result;

                        if (serviceResult == PartyServiceResult.FailedNotInParty) result = PartyResultCode.WithdrawParty_NotJoined;
                        else if (serviceResult != PartyServiceResult.Ok) result = PartyResultCode.WithdrawParty_Unknown;

                        response.WriteByte((byte)result);

                        if (result == PartyResultCode.WithdrawParty_Done) return;

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
