using Edelstein.Common.Gameplay.Packets;
using Edelstein.Common.Gameplay.Social;
using Edelstein.Common.Utilities.Packets;
using Edelstein.Protocol.Gameplay.Contracts;
using Edelstein.Protocol.Gameplay.Game;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay.Game.Plugs;

public class NotifyFriendUpdateListPlug : IPipelinePlug<NotifyFriendUpdateList>
{
    private readonly IGameStage _stage;

    public NotifyFriendUpdateListPlug(IGameStage stage) => _stage = stage;
    
    public async Task Handle(IPipelineContext ctx, NotifyFriendUpdateList message)
    {
        var user = await _stage.Users.Retrieve(message.CharacterID);
        if (user == null) return;
        user.Friends = message.FriendList;

        var p = new PacketWriter(PacketSendOperations.FriendResult);
        p.WriteByte((byte)FriendResultOperations.SetFriend_Done);
        p.WriteByte((byte)message.FriendList.Records.Count);
        foreach (var friend in message.FriendList.Records.Values)
            p.WriteFriendInfo(friend);
        foreach (var friend in message.FriendList.Records.Values)
            p.WriteInt(0);
        _ = user.Dispatch(p.Build());
    }
}
