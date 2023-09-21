using Edelstein.Common.Gameplay.Packets;
using Edelstein.Common.Gameplay.Social;
using Edelstein.Protocol.Gameplay.Game.Contracts;
using Edelstein.Protocol.Gameplay.Game.Objects.User;
using Edelstein.Protocol.Utilities.Packets;
using Microsoft.Extensions.Logging;

namespace Edelstein.Common.Gameplay.Game.Handlers;

public class PartyResultHandler : AbstractFieldHandler
{
    private readonly ILogger _logger;
    
    public PartyResultHandler(ILogger<PartyResultHandler> logger) => _logger = logger;
    
    public override short Operation => (short)PacketRecvOperations.PartyResult;
    
    protected override Task Handle(IFieldUser user, IPacketReader reader)
    {
        var type = (PartyResultOperations)reader.ReadByte();

        switch (type)
        {
            case PartyResultOperations.InvitePartyAccepted:
                return user.StageUser.Context.Pipelines.FieldOnPacketPartyInviteAcceptResult.Process(new FieldOnPacketPartyInviteAcceptResult(
                    user,
                    reader.ReadInt()
                ));
            case PartyResultOperations.InvitePartyRejected:
                return user.StageUser.Context.Pipelines.FieldOnPacketPartyInviteRejectResult.Process(new FieldOnPacketPartyInviteRejectResult(
                    user,
                    reader.ReadInt()
                ));
            default:
                _logger.LogWarning("Unhandled party result type {Type}", type);
                break;
        }
        
        return Task.CompletedTask;
    }
}
