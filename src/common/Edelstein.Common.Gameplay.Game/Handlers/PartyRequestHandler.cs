using Edelstein.Common.Gameplay.Packets;
using Edelstein.Common.Gameplay.Social;
using Edelstein.Protocol.Gameplay.Game.Contracts;
using Edelstein.Protocol.Gameplay.Game.Objects.User;
using Edelstein.Protocol.Utilities.Packets;
using Microsoft.Extensions.Logging;

namespace Edelstein.Common.Gameplay.Game.Handlers;

public class PartyRequestHandler : AbstractFieldHandler
{
    private readonly ILogger _logger;
    
    public PartyRequestHandler(ILogger<PartyRequestHandler> logger) => _logger = logger;
    
    public override short Operation => (short)PacketRecvOperations.PartyRequest;
    
    protected override Task Handle(IFieldUser user, IPacketReader reader)
    {
        var type = (PartyRequestOperations)reader.ReadByte();

        switch (type)
        {
            case PartyRequestOperations.CreateParty:
                return user.StageUser.Context.Pipelines.FieldOnPacketPartyCreateRequest.Process(new FieldOnPacketPartyCreateRequest(
                    user
                ));
            case PartyRequestOperations.LeaveParty:
                return user.StageUser.Context.Pipelines.FieldOnPacketPartyLeaveRequest.Process(new FieldOnPacketPartyLeaveRequest(
                    user
                ));
            case PartyRequestOperations.InviteParty:
                return user.StageUser.Context.Pipelines.FieldOnPacketPartyInviteRequest.Process(new FieldOnPacketPartyInviteRequest(
                    user,
                    reader.ReadString()
                ));
            case PartyRequestOperations.KickParty:
                return user.StageUser.Context.Pipelines.FieldOnPacketPartyKickRequest.Process(new FieldOnPacketPartyKickRequest(
                    user,
                    reader.ReadInt()
                ));
            case PartyRequestOperations.ChangePartyLeader:
                return user.StageUser.Context.Pipelines.FieldOnPacketPartyChangeLeaderRequest.Process(new FieldOnPacketPartyChangeLeaderRequest(
                    user,
                    reader.ReadInt()
                ));
            default:
                _logger.LogWarning("Unhandled party request type {Type}", type);
                break;
        }
        
        return Task.CompletedTask;
    }
}
