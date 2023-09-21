using Edelstein.Common.Gameplay.Packets;
using Edelstein.Common.Gameplay.Social;
using Edelstein.Common.Utilities.Packets;
using Edelstein.Protocol.Gameplay.Contracts;
using Edelstein.Protocol.Gameplay.Game;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay.Game.Plugs;

public class NotifyFriendInvitedPlug : IPipelinePlug<NotifyFriendInvited>
{
    private readonly IGameStage _stage;

    public NotifyFriendInvitedPlug(IGameStage stage) => _stage = stage;
    
    public async Task Handle(IPipelineContext ctx, NotifyFriendInvited message)
    {
        var user = await _stage.Users.Retrieve(message.FriendID);
        if (user == null) return;
        var p = new PacketWriter(PacketSendOperations.FriendResult);
        p.WriteByte((byte)FriendResultOperations.Invite);
        p.WriteInt(message.InviterID);
        p.WriteString(message.InviterName);
        p.WriteInt(message.InviterLevel);
        p.WriteInt(message.InviterJob);
        p.WriteFriendInfo(message.Friend);
        p.WriteBool(false);
        _ = user.Dispatch(p.Build());
    }
}
