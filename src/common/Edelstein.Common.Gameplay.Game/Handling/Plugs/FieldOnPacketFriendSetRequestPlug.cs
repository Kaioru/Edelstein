using Edelstein.Common.Gameplay.Handling;
using Edelstein.Common.Gameplay.Social;
using Edelstein.Common.Utilities.Packets;
using Edelstein.Protocol.Gameplay.Game.Contracts;
using Edelstein.Protocol.Services.Social;
using Edelstein.Protocol.Services.Social.Contracts;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay.Game.Handling.Plugs;

public class FieldOnPacketFriendSetRequestPlug : IPipelinePlug<FieldOnPacketFriendSetRequest>
{
    private readonly IFriendService _service;
    
    public FieldOnPacketFriendSetRequestPlug(IFriendService service) => _service = service;
    
    public async Task Handle(IPipelineContext ctx, FieldOnPacketFriendSetRequest message)
    {
        var existing = message.User.StageUser.Friends?.Records.Values
            .FirstOrDefault(f => f.FriendName == message.FriendName);
        var response = existing == null
            ? await _service.Invite(new FriendInviteRequest(
                message.User.Character.ID,
                message.User.Character.Name,
                message.User.Character.Level,
                message.User.Character.Job,
                message.User.StageUser.Context.Options.ChannelID,
                message.FriendName,
                message.FriendGroup
            ))
            : await _service.UpdateGroup(new FriendUpdateGroupRequest(
                message.User.Character.ID,
                existing.FriendID,
                message.FriendGroup
            ));

        if (response.Result == FriendResult.Success) return;
        
        var result = response.Result switch
        {
            FriendResult.FailedMaxSlotMe => FriendResultOperations.SetFriendFullMe,
            FriendResult.FailedMaxSlotOther => FriendResultOperations.SetFriendFullOther,
            FriendResult.FailedAlreadyAdded => FriendResultOperations.SetFriendAlreadySet,
            FriendResult.FailedMaster => FriendResultOperations.SetFriendMaster,
            FriendResult.FailedCharacterNotFound => FriendResultOperations.SetFriendUnknownUser,
            _ => FriendResultOperations.SetFriendUnknown
        };
        var p = new PacketWriter(PacketSendOperations.FriendResult);
        p.WriteByte((byte)result);
        if (result == FriendResultOperations.SetFriendUnknown)
            p.WriteBool(false);
        await message.User.Dispatch(p.Build());
    }
}
