using Edelstein.Common.Gameplay.Packets;
using Edelstein.Common.Gameplay.Social;
using Edelstein.Protocol.Gameplay.Game.Contracts;
using Edelstein.Protocol.Gameplay.Game.Objects.User;
using Edelstein.Protocol.Utilities.Packets;
using Microsoft.Extensions.Logging;

namespace Edelstein.Common.Gameplay.Game.Handlers;

public class FriendRequestHandler : AbstractFieldHandler
{
    private readonly ILogger _logger;
    
    public FriendRequestHandler(ILogger<FriendRequestHandler> logger) => _logger = logger;
    
    public override short Operation => (short)PacketRecvOperations.FriendRequest;
    
    protected override Task Handle(IFieldUser user, IPacketReader reader)
    {
        var type = (FriendRequestOperations)reader.ReadByte();

        switch (type)
        {
            case FriendRequestOperations.SetFriend:
                return user.StageUser.Context.Pipelines.FriendOnPacketFriendSetRequest.Process(new FriendOnPacketFriendSetRequest(
                    user,
                    reader.ReadString(),
                    reader.ReadString()
                ));
            case FriendRequestOperations.AcceptFriend:
                return user.StageUser.Context.Pipelines.FriendOnPacketFriendAcceptRequest.Process(new FriendOnPacketFriendAcceptRequest(
                    user,
                    reader.ReadInt()
                ));
            case FriendRequestOperations.DeleteFriend:
                return user.StageUser.Context.Pipelines.FriendOnPacketFriendDeleteRequest.Process(new FriendOnPacketFriendDeleteRequest(
                    user,
                    reader.ReadInt()
                ));
            default:
                _logger.LogWarning("Unhandled friend request type {Type}", type);
                break;
        }
        
        return Task.CompletedTask;
    }
}
