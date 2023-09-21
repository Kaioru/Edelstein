using Edelstein.Common.Gameplay.Packets;
using Edelstein.Common.Gameplay.Social;
using Edelstein.Common.Utilities.Packets;
using Edelstein.Protocol.Gameplay.Game.Contracts;
using Edelstein.Protocol.Services.Social;
using Edelstein.Protocol.Services.Social.Contracts;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay.Game.Plugs;

public class FriendOnPacketFriendDeleteRequestPlug : IPipelinePlug<FriendOnPacketFriendDeleteRequest>
{
    private readonly IFriendService _service;
    
    public FriendOnPacketFriendDeleteRequestPlug(IFriendService service) => _service = service;
    
    public async Task Handle(IPipelineContext ctx, FriendOnPacketFriendDeleteRequest message)
    {
        var response = await _service.Delete(new FriendDeleteRequest(
            message.User.Character.ID,
            message.FriendID
        ));
        
        if (response.Result == FriendResult.Success) return;
        
        var p = new PacketWriter(PacketSendOperations.FriendResult);
        p.WriteByte((byte)FriendResultOperations.DeleteFriendUnknown);
        p.WriteBool(false);
        await message.User.Dispatch(p.Build());
    }
}
