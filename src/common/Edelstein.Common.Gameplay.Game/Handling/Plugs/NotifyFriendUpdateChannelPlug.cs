using Edelstein.Common.Gameplay.Handling;
using Edelstein.Common.Gameplay.Social;
using Edelstein.Common.Utilities.Packets;
using Edelstein.Protocol.Gameplay.Contracts;
using Edelstein.Protocol.Gameplay.Game;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay.Game.Handling.Plugs;

public class NotifyFriendUpdateChannelPlug : IPipelinePlug<NotifyFriendUpdateChannel>
{
    private readonly IGameStage _stage;
    
    public NotifyFriendUpdateChannelPlug(IGameStage stage) => _stage = stage;
    
    public async Task Handle(IPipelineContext ctx, NotifyFriendUpdateChannel message)
    {
        var users = await _stage.Users.RetrieveAll();

        foreach (var user in users)
        {
            if (!(user.Friends?.Records.TryGetValue(message.CharacterID, out var friend) ?? false)) continue;
            friend.ChannelID = message.ChannelID;
            if (friend.Flag > 0) return;

            using var packet = new PacketWriter(PacketSendOperations.FriendResult);
            packet.WriteByte((byte)FriendResultOperations.Notify);
            packet.WriteInt(message.CharacterID);
            packet.WriteBool(false);
            packet.WriteInt(message.ChannelID);
            _ = user.Dispatch(packet.Build());
        }
    }
}
