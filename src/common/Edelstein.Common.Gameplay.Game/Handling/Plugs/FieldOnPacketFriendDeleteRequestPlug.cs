using Edelstein.Common.Gameplay.Handling;
using Edelstein.Common.Gameplay.Social;
using Edelstein.Common.Utilities.Packets;
using Edelstein.Protocol.Gameplay.Game.Contracts;
using Edelstein.Protocol.Services.Social;
using Edelstein.Protocol.Services.Social.Contracts;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay.Game.Handling.Plugs;

public class FieldOnPacketFriendDeleteRequestPlug : IPipelinePlug<FieldOnPacketFriendDeleteRequest>
{
    private readonly IFriendService _service;
    
    public FieldOnPacketFriendDeleteRequestPlug(IFriendService service) => _service = service;
    
    public async Task Handle(IPipelineContext ctx, FieldOnPacketFriendDeleteRequest message)
    {
        var response = await _service.Delete(new FriendDeleteRequest(
            message.User.Character.ID,
            message.FriendID
        ));
        
        if (response.Result == FriendResult.Success) return;
        
        using var packet = new PacketWriter(PacketSendOperations.FriendResult);
        packet.WriteByte((byte)FriendResultOperations.DeleteFriendUnknown);
        packet.WriteBool(false);
        await message.User.Dispatch(packet.Build());
    }
}
