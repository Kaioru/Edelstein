using Edelstein.Common.Gameplay.Packets;
using Edelstein.Common.Gameplay.Social;
using Edelstein.Common.Utilities.Packets;
using Edelstein.Protocol.Gameplay.Game.Contracts;
using Edelstein.Protocol.Services.Social;
using Edelstein.Protocol.Services.Social.Contracts;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay.Game.Plugs;

public class FriendOnPacketFriendAcceptRequestPlug : IPipelinePlug<FriendOnPacketFriendAcceptRequest>
{
    private readonly IFriendService _service;
    
    public FriendOnPacketFriendAcceptRequestPlug(IFriendService service) => _service = service;
    
    public async Task Handle(IPipelineContext ctx, FriendOnPacketFriendAcceptRequest message)
    {
        var response = await _service.InviteAccept(new FriendInviteAcceptRequest(
            message.FriendID,
            message.User.Character.ID,
            message.User.StageUser.Context.Options.ChannelID
        ));
        
        if (response.Result == FriendResult.Success) return;
        
        var p = new PacketWriter(PacketSendOperations.FriendResult);
        p.WriteByte((byte)FriendResultOperations.AcceptFriendUnknown);
        p.WriteBool(false);
        await message.User.Dispatch(p.Build());
    }
}
