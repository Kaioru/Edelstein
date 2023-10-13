using Edelstein.Common.Gameplay.Handling;
using Edelstein.Common.Gameplay.Social;
using Edelstein.Common.Utilities.Packets;
using Edelstein.Protocol.Gameplay.Contracts;
using Edelstein.Protocol.Gameplay.Game;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay.Game.Handling.Plugs;

public class NotifyFriendInvitedPlug : IPipelinePlug<NotifyFriendInvited>
{
    private readonly IGameStage _stage;

    public NotifyFriendInvitedPlug(IGameStage stage) => _stage = stage;
    
    public async Task Handle(IPipelineContext ctx, NotifyFriendInvited message)
    {
        var user = await _stage.Users.Retrieve(message.FriendID);
        if (user == null) return;
        using var packet = new PacketWriter(PacketSendOperations.FriendResult);
        packet.WriteByte((byte)FriendResultOperations.Invite);
        packet.WriteInt(message.InviterID);
        packet.WriteString(message.InviterName);
        packet.WriteInt(message.InviterLevel);
        packet.WriteInt(message.InviterJob);
        packet.WriteFriendInfo(message.Friend);
        packet.WriteBool(false);
        _ = user.Dispatch(packet.Build());
    }
}
