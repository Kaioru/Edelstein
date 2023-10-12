using Edelstein.Common.Gameplay.Handling;
using Edelstein.Common.Gameplay.Social;
using Edelstein.Protocol.Gameplay.Game.Contracts;
using Edelstein.Protocol.Gameplay.Game.Objects.User;
using Edelstein.Protocol.Utilities.Packets;
using Microsoft.Extensions.Logging;

namespace Edelstein.Common.Gameplay.Game.Handling.Packets;

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
                return user.StageUser.Context.Pipelines.FieldOnPacketFriendSetRequest.Process(new FieldOnPacketFriendSetRequest(
                    user,
                    reader.ReadString(),
                    reader.ReadString()
                ));
            case FriendRequestOperations.AcceptFriend:
                return user.StageUser.Context.Pipelines.FieldOnPacketFriendAcceptRequest.Process(new FieldOnPacketFriendAcceptRequest(
                    user,
                    reader.ReadInt()
                ));
            case FriendRequestOperations.DeleteFriend:
                return user.StageUser.Context.Pipelines.FieldOnPacketFriendDeleteRequest.Process(new FieldOnPacketFriendDeleteRequest(
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
