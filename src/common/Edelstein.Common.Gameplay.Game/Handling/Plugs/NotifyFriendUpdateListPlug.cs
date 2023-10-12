using Edelstein.Common.Gameplay.Handling;
using Edelstein.Common.Gameplay.Social;
using Edelstein.Common.Utilities.Packets;
using Edelstein.Protocol.Gameplay.Contracts;
using Edelstein.Protocol.Gameplay.Game;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay.Game.Handling.Plugs;

public class NotifyFriendUpdateListPlug : IPipelinePlug<NotifyFriendUpdateList>
{
    private readonly IGameStage _stage;

    public NotifyFriendUpdateListPlug(IGameStage stage) => _stage = stage;
    
    public async Task Handle(IPipelineContext ctx, NotifyFriendUpdateList message)
    {
        var user = await _stage.Users.Retrieve(message.CharacterID);
        if (user == null) return;
        user.Friends = message.FriendList;

        using var packet = new PacketWriter(PacketSendOperations.FriendResult);
        packet.WriteByte((byte)FriendResultOperations.SetFriend_Done);
        packet.WriteByte((byte)message.FriendList.Records.Count);
        foreach (var friend in message.FriendList.Records.Values)
            packet.WriteFriendInfo(friend);
        foreach (var friend in message.FriendList.Records.Values)
            packet.WriteInt(0);
        _ = user.Dispatch(packet.Build());
    }
}
